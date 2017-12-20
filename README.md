# AutoGravityUnitTestSample
AutoGravity Coding Challenge

* Utilizes Selenium with C# Bindings and NUnit, which is a unit testing framework for C#
* IDE used: Visual Studio 2017
* Automation was tested using the FireFox Driver

* Code automates the task of creating a credit application on https://www.autogravity.com
* The automation randomly selects the following while creating the application:
  1. new/used option
  2. car make
  3. car model
  4. trim (if applicable)
  5. finance option (lease or loan)
  6. dealership location (if applicable)

Instructions to clone:
1. using the command line (e.g. git bash) enter in the following:
`git clone "https://github.com/hlimbo/AutoGravityUnitTestSample.git`

Instructions to run test:
1. Open Visual Studio Project
2. Click on Test -> Windows -> Test Explorer to view the Test Explorer pane
3. With the Test Explorer pane opened, right click on `Create_Credit_App_Firefox` to run test

* Note: an Info.log file is created if it does not exist and appends information about the random car selected for the credit application. The file Info.log can be found under bin/Debug folder.
