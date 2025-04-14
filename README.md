# SchoolAdministrator
Scenario

A student administration system is required for use by a departmental administrator.  A student is registered on a degree programme and becomes a member of a particular student cohort.  A degree programme is either a one year programme or a two year programme.  A one year programme contains six modules and a two year programme contains twelve modules.  Each module contains one or more assessments. A student may gain a mark for each assessment of a module to provide an overall module mark and then a programme mark. The system should allow the operations listed below.  Data must persist between invocations of the system.

 

Details

 

A student is registered on a degree programme and becomes a member of a particular student cohort.  A student cohort is the set of students registered for a given degree programme in a given academic session (academic year).  A student has an ID and a name, the ID begins with the year of the cohort followed by unique 6 digit string.  A degree programme has a 6 digit programme identifier and a programme title.  A degree programme is either a one, two or three year programme.  A one year programme contains six modules, a two year programme therefore contains twelve modules.

 

A module has a 5-digit module code and a module title.  Some of the modules of a programme are determined by that program and some are chosen as options by the student registered on that programme.  No two degree programs may have the same set of modules. 

 

Each module contains one or more assessments. Each assessment of a module has a maximum mark which is less than or equal to 100.  The sum of the maximum marks for the assessments of any module is 100.  An assessment may be shared across more than one module.  A student may gain a mark for each assessment of a module, the sum of which is the overall module mark.   The module result is Pass if the module mark is greater than or equal to 50.  If the module mark is less than 50 but greater than or equal to 45 then the module result is PassCompensation.  Otherwise the module result is Fail.  A module result is undefined if there is no mark for any assessment in that module.  The average mark across all the modules in a programme, as an integer percentage, is the programme mark.  The programme mark is undefined if any module mark is undefined.  The programme result is Pass if the programme mark is greater than or equal to 50, Distinction if the programme mark is greater than or equal to 70 and Fail if less than 50.
