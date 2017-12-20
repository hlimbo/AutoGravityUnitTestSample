# AutoGravityUnitTestSample

## Overview
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

## Instructions to clone:
1. using the command line (e.g. git bash) enter in the following:
`git clone "https://github.com/hlimbo/AutoGravityUnitTestSample.git`

## Instructions to run test:
1. Open Visual Studio Project
2. Click on Test -> Windows -> Test Explorer to view the Test Explorer pane
3. With the Test Explorer pane opened, right click on `Create_Credit_App_Firefox` to run test

* Note: an Info.log file is created if it does not exist and appends information about the random car selected for the credit application. The file Info.log can be found under bin/Debug folder.


## Example Output of Info.log

```
---------
New/Used: New
MakesCollection Count: 48
Make Type Selected: Honda
ModelsCollection Count: 13
Car Model Selected: Odyssey
Location Modal is Displayed
Inventory selected: 2018 Honda Odyssey EX-L Auto
Dealership Name: DCH Honda of Temecula
Finance Toggle Count: 1
finance type: Loan
Test Succeeded!
---------
New/Used: Used
MakesCollection Count: 42
Make Type Selected: Mitsubishi
Used ModelsCollection Count: 12
Used Car Model Selected: Mirage
Location Modal is Displayed
Inventory selected: 2014 Mitsubishi Mirage 4dr HB Man DE
Dealership Name: AutoNation Ford Torrance
used finance type: Review Loan Details
Test Succeeded!
---------
New/Used: New
MakesCollection Count: 48
Make Type Selected: Audi
ModelsCollection Count: 30
Car Model Selected: TT Roadster
Location Modal is Displayed
Trim selected: 2.0 TFSI
Finance Toggle Count: 2
finance type: LOAN
Dealership name selected: Audi Mission Viejo
display: True
display: False
display: True
Test Succeeded!
```
