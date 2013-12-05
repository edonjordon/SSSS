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
    public static class RuleCheckerOnEdit
    {
        private static List<Section> sectionsInSemester;
        private static int selectedSemesterId;
        private static SSSSContext dbContext;
        private static Section sectionToCheck;
        private static List<Section> sectionsToUpdate;
        public static List<SectionConflict> conflictsToAdd;

        public static onEditWrapper initialize(Section aSection, int aSemesterId, List<Section> sectionList, SSSSContext passedContext)
        {
            dbContext = passedContext;
            sectionsInSemester = sectionList;
            sectionToCheck = aSection;
            selectedSemesterId = aSemesterId;
            return checkSection();

        }

        public class onEditWrapper
        {

            public List<Section> listOfSections;
            public Section sectionChecked;
            public List<SectionConflict> conflictList;



            public onEditWrapper(List<Section> sectionList, Section aSection, List<SectionConflict> aListOfConflicts)
            {
                listOfSections = sectionList;
                sectionChecked = aSection;
                conflictList = aListOfConflicts;

            }
        }

        //ON INSERT
        public static onEditWrapper checkSection()
        {
            sectionsToUpdate = new List<Section>();
            conflictsToAdd = new List<SectionConflict>();
            foreach (Section currentSection in sectionsInSemester)
            {

                List<string> listOfConflictsForCurrent = new List<string>();
                List<string> listOfConflictsForChecked = new List<string>();
                List<Course> courseToCheck = dbContext.Course.Where(x => x.CourseId == sectionToCheck.CourseId).ToList();
                SectionConflict currentConflict = new SectionConflict();
                SectionConflict checkedConflict = new SectionConflict();



                if (currentSection.StartTime == sectionToCheck.StartTime)
                {
                    if (currentSection.Monday == sectionToCheck.Monday &&
                        currentSection.Tuesday == sectionToCheck.Tuesday &&
                        currentSection.Wednesday == sectionToCheck.Wednesday &&
                        currentSection.Thursday == sectionToCheck.Thursday &&
                        currentSection.Friday == sectionToCheck.Friday)
                    {

                        if (currentSection.RoomId == sectionToCheck.RoomId)
                        {

                            currentSection.HasConflicts = true;
                            //listOfConflictsForCurrent.Add("Room conflict!");
                            currentConflict.roomConflict = true;


                            sectionToCheck.HasConflicts = true;
                            //listOfConflictsForChecked.Add("Room conflict!");
                            checkedConflict.roomConflict = true;

                        }
                        if (currentSection.InstructorId == sectionToCheck.InstructorId)
                        {
                            currentSection.HasConflicts = true;
                            //listOfConflictsForCurrent.Add("Instructor conflict!");
                            currentConflict.instructorConflict = true;

                            sectionToCheck.HasConflicts = true;
                            //listOfConflictsForChecked.Add("Instructor conflict!");
                            checkedConflict.instructorConflict = true;

                        }
                        if (currentSection.SectionCourse.CourseNumber[0] == courseToCheck[0].CourseNumber[0])
                        {
                            //theseare here for debugging 
                            char did = currentSection.SectionCourse.CourseNumber[0];
                            char dont = courseToCheck[0].CourseNumber[0];


                            currentSection.HasConflicts = true;
                            //listOfConflictsForCurrent.Add("Cohort conflict!");
                            currentConflict.cohortConflict = true;

                            sectionToCheck.HasConflicts = true;
                            //listOfConflictsForChecked.Add("Cohort conflict!");
                            checkedConflict.cohortConflict = true;

                        }


                    }


                }
                if (sectionToCheck.HasConflicts == true)
                {
                    currentConflict.parentSection = currentSection;
                    currentConflict.sectionConflicted = sectionToCheck;
                    //currentConflict.ConflictList = listOfConflictsForCurrent;
                    conflictsToAdd.Add(currentConflict);

                    checkedConflict.parentSection = sectionToCheck;
                    checkedConflict.sectionConflicted = currentSection;
                    //checkedConflict.ConflictList = listOfConflictsForCurrent;
                    conflictsToAdd.Add(checkedConflict);
                    sectionsToUpdate.Add(currentSection);
                    //listOfConflicts
                }
            }

            onEditWrapper ret = new onEditWrapper(sectionsToUpdate, sectionToCheck, conflictsToAdd);
            return ret;
        }
    }
}