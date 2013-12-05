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
    public class SectionsOnInsert
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
        public static void initialize(Section aSection, int aSemesterId, SSSSContext passedContext)
        {
            dbContext = passedContext;
            sectionToCheck = aSection;
            selectedSemesterId = aSemesterId;
            sectionsInSemester = dbContext.Section.Where(x => x.SemesterId == sectionToCheck.SemesterId).ToList();
            returnWrapper checkResults = checkSection();

            dbContext.Section.Add(checkResults.sectionChecked);

            foreach (Section s in checkResults.listOfSections)
            {
                dbContext.Entry(s).State = EntityState.Modified;
            }

            foreach (SectionConflict c in checkResults.conflictList)
            {
                dbContext.SectionConflict.Add(c);
            }

            

            dbContext.SaveChanges();
            //db.Dispose();

        }




        //ON INSERT
        private static returnWrapper checkSection()
        {
            //initialize the lists needed to hold the sections and conflicts that need to be modified and added
            sectionsToUpdate = new List<Section>();
            conflictsToAdd = new List<SectionConflict>();

            //Main for loop that loops through every section within the semester and compares it to the section that was passed in
            foreach(Section currentSection in sectionsInSemester){
               
                //Initialize a list of conflicts for the current section
                List<SectionConflict> listOfConflictsForCurrentSection = new List<SectionConflict>();
                listOfConflictsForCurrentSection = dbContext.SectionConflict.Where(x => x.sectionConflicted.SectionId == currentSection.SectionId).ToList();
                //Initialize a list for conflicts that the section we're checking has.
                List<SectionConflict> listOfConflictsForSectionToCheck= new List<SectionConflict>();
                listOfConflictsForSectionToCheck = dbContext.SectionConflict.Where(x => x.parentSection.SectionId == sectionToCheck.SectionId).ToList();

                //Pull the course to check, this will be used to check for cohorts and pre-requesites
                List<Course> courseToCheck = dbContext.Course.Where(x => x.CourseId == sectionToCheck.CourseId).ToList();
                //Create the conflicts for the CurrentSection and the Sectionto check
                SectionConflict currentConflict = new SectionConflict();
                SectionConflict checkedConflict = new SectionConflict();


                //check to see if the section's times match
                if (currentSection.StartTime == sectionToCheck.StartTime)
                {
                    //check to see if the section's days taught match up
                    if(currentSection.Monday == sectionToCheck.Monday &&
                        currentSection.Tuesday == sectionToCheck.Tuesday &&
                        currentSection.Wednesday == sectionToCheck.Wednesday &&
                        currentSection.Thursday == sectionToCheck.Thursday &&
                        currentSection.Friday == sectionToCheck.Friday){
                        
                        //Check to see if the rooms match, if it does, then mark down that the conflict should be added
                        //Also, set the hasConflicts to True for the wrapper, and both sections
                        if (currentSection.RoomId == sectionToCheck.RoomId)
                        {
                            currentSection.HasConflicts = true;
                            currentConflict.roomConflict = true;

                            sectionToCheck.HasConflicts = true;
                            checkedConflict.roomConflict = true;                            
				        }
                        //Also, set the hasConflicts to True for the wrapper, and both sections
                        if (currentSection.InstructorId == sectionToCheck.InstructorId)
                        {
                            currentSection.HasConflicts = true;
                            currentConflict.instructorConflict = true;
                            
                            sectionToCheck.HasConflicts = true;
                            checkedConflict.instructorConflict = true;                            
				        }
                        //Also, set the hasConflicts to True for the wrapper, and both sections
                        if (currentSection.SectionCourse.CourseNumber[0] == courseToCheck[0].CourseNumber[0])
                        {
                            currentSection.HasConflicts = true;
                            currentConflict.cohortConflict = true;
                            
                            sectionToCheck.HasConflicts = true;
                            checkedConflict.cohortConflict = true;                            
				        }


                    }

                    			
			    }
                if(sectionToCheck.HasConflicts == true)
                {
                    //this is where we fill out those two sectionConflicts we made earlier

                    //set current conflict's parent and conflicted sections
                    currentConflict.parentSection = currentSection;
                    currentConflict.sectionConflicted = sectionToCheck;
                   
                    //Add it to the list, we'll add them all at once at a later time
                    conflictsToAdd.Add(currentConflict);

                    //Same thing
                    checkedConflict.parentSection = sectionToCheck;
                    checkedConflict.sectionConflicted = currentSection;
                    
                    conflictsToAdd.Add(checkedConflict);
                    //We need to update the current sectiona as well.
                    sectionsToUpdate.Add(currentSection);
                    
                }
		    }

            returnWrapper ret = new returnWrapper(sectionsToUpdate, sectionToCheck, conflictsToAdd);
            if(sectionToCheck.HasConflicts == true){
                ret.hasConflicts = true;
            }
            return ret;
        }
    }


}
