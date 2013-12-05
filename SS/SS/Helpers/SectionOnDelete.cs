using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Models;

namespace SS.Helpers
{
    public class SectionOnDelete
    {
                //Class variables
        //We'll need to gather the section being inserted, and get the sections that are in the semester
        //We also need to pass in our DbContext
        //We're using global variables here, since we'll be interacting with one section at a time.
        private static List<Section> sectionsInSemester;
        private static int selectedSemesterId;
        private static SSSSContext dbContext;
        private static Section sectionToCheck;

        //This is a list of sections that need to be updated, if the section being inserted causes conflicts
        private static List<Section> sectionsToUpdate;
        //This is a list of the conflicts that need to be added into the database
        public static List<SectionConflict> conflictsToAdd;

        //WRAPPER CLASS
        //This wrapper class is used to get a complex return type from checkSection();
        //It has a list of sections that need to be updated
        //It has a list of conflicts that need to be created
        //It has the section that was passed in to check
        public class returnWrapper{

            public List<Section> listOfSections;
            public List<SectionConflict> conflictList;
            public Section sectionChecked; 
            public Boolean hasConflicts;

            //Constructor
            public returnWrapper(List<Section> sectionList, Section aSection, List<SectionConflict> aListOfConflicts)
            {
                listOfSections = sectionList;
                sectionChecked = aSection;
                conflictList = aListOfConflicts;
                hasConflicts = false;


            }
        }

        //INITIALIZE
        //this is the method you call to interact with the class
        public static void initialize(int sectionId, SSSSContext passedContext)
        {
            dbContext = passedContext;
            sectionToCheck = dbContext.Section.Find(sectionId);
            selectedSemesterId = sectionToCheck.SemesterId;
            sectionsInSemester = dbContext.Section.Where(x => x.SemesterId == sectionToCheck.SemesterId && x.SectionId != sectionId).ToList();
            checkSection();        

            dbContext.SaveChanges();

        }




        //ON INSERT
        private static void checkSection()
        {
            
            //get both conflicts
            //one for section being deleted
            //one for the section the conflict was related to. 
            //List<SectionConflict> conflicts = dbContext.SectionConflict.Where(x => x.parentSection.SectionId == sectionToCheck.SectionId).ToList();
            //List<SectionConflict> conflicts2 = dbContext.SectionConflict.Where(x => x.sectionConflicted.SectionId == sectionToCheck.SectionId).ToList();

            List<SectionConflict> conflictsForSectionToCheck = dbContext.SectionConflict.Where(x => x.parentSection.SectionId == sectionToCheck.SectionId).ToList();
            List<SectionConflict> conflictsForCurrentSection = new List<SectionConflict>();
            List<SectionConflict> conflictsToCheckForCurrentSection = new List<SectionConflict>();
            foreach (SectionConflict c in conflictsForSectionToCheck)
            {
                dbContext.SectionConflict.Remove(c);
            }

            foreach (Section s in sectionsInSemester)
            {
                //Get list of conflicts part of the sectionTocheck
                conflictsForCurrentSection = dbContext.SectionConflict.Where(x => x.parentSection.SectionId == s.SectionId && x.parentSection.SectionId != sectionToCheck.SectionId).ToList();
                foreach (SectionConflict c in conflictsForCurrentSection)
                {
                    if (c.sectionConflicted.SectionId == sectionToCheck.SectionId)
                    {
                        dbContext.SectionConflict.Remove(c);
                    }
                }
                dbContext.SaveChanges();
                conflictsForCurrentSection = dbContext.SectionConflict.Where(x => x.parentSection.SectionId == s.SectionId && x.parentSection.SectionId != sectionToCheck.SectionId).ToList();
                //conflictsToCheckForCurrentSection = dbContext.SectionConflict.Where(x => x.parentSection.SectionId == s.SectionId && x.sectionConflicted.SectionId != null).ToList();

                if (conflictsForCurrentSection.Count() == 0)
                        {
                            s.HasConflicts = false;
                            dbContext.Entry(s).State = EntityState.Modified;
                        }
            }

            dbContext.Section.Remove(sectionToCheck);
            
        }
    }
}