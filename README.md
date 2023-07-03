# Migraine Diary

## About
This is educational project for SoftUni's **_ASP.NET Advanced_** course, built using **ASP.NET MVC**, **Entity framework core**, **Microsoft SQL Server**, **JavaScript**, **HTML**, **CSS**, **Bootstrap**.\
\
The application represents _online_ _diary_ for keeping track of migraine headaches by migraine sufferers and their neurologist which is important for diagnosis, profilaxis and treatment. Beside this patients can also register 2 types of scale - Headache Impact Test (HIT-6) and Zung's scale for anxiety. Both scales are widely used for self assessment and for research purposes aswell.\
Users also can find information about clinical trials and articles related to migraine - approved new drugs on the market, new findings about etiology, pathophysiology, etc... 

## Users (Roles)
There are 4 types of users - Guest, Patient, Doctor and Admin user.

1. ### Guest user's permissions (not logged in)
    * Read articles
    * See information about registered clinical trials by Doctor users
    * Send message to site's administrators using the Contact form
    
2. ### Patient user's permissions
    * Register headaches
    * Register HIT-6 scale
    * Register Zung's scale for anxiety
    * Share own registered headaches/scales with Doctor user
    * Edit own registered scales
    * Delete own registered scales
    * See information about registered clinical trials by Doctor users

3. ### Doctor user's permissions
    * Everything which Patient user can do
    * Register clinical trials
    * Edit own registered clinical trials
    * Delete own registered clinical trials
    * View shared with him headaches and scales
    
4. ### Admin user's permissions
    * Everything other user's can do except accessing patients clinical information
    * View information about registered users and their roles
    * Assign roles to users
    * Remove users from role
    * Add articles
    * Edit articles
    * Delete articles
    * View messages sent using Contacts form
    
## Implemented functionalities
* Pagination
* Sorting by latest/newly added
* Real-time notifications about received messages (Fetching every 60 seconds)
* Search & suggest
* Integrated [Froala](https://froala.com) text editor

## Prerequisites
1. In order to run the application you should add 3 files into **..\src\MVCProject.Web\bin\Debug\net6.0** named *"adminpassword.txt"*, *"testdoctorpassword.txt"* and *"testuserpassword.txt"*. In each of the files on the first row must be written password for seeded account which must contain at least 1 capital letter, 1 lowercase letter, 1 number and 1 symbol (min length 6 symbols), e.g. *T3stP@tient*.
2. Running Microsoft SQL Server.

## Seeded accounts nicknames
1. **Admin**
2. **TestDoctor**
3. **TestPatient**
