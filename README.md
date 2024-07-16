# Project Introduction

## Contents
- [Project Description](#project-description)
- [Major Features (Epics)](#major-features-epics)
- [Technologies Used](#technologies-used)
- [Team Member Assignment Table](#team-member-assignment-table)

## Project Description
ManageBook is a book management application designed to help users easily manage their book collections. The application allows users to add, edit, delete, and search for book information efficiently. Additionally, it supports tracking borrowing/returning statuses and managing borrower information.

## Major Features (Epics)
   
1. **Borrower Management**:
   - Add, edit, and delete borrower information.
   - Track borrowing/returning statuses.

2. **User Role Management**:
   - **Admin/Manager**: Complete CRUD screens for tables (e.g., CRUD Book, CRUD Category) with validation.
   - **Staff**: Allow creating, editing, and searching, but do not allow deletion.
   - **Member**: Only view and search capabilities.

3. **View Books by Category**:
   - A screen allowing all roles to view books by category.
   - When a user selects a category from the dropdown, the system displays a list of books belonging to that category.
   - When a user selects a value from the main table dropdown, the system displays a list of records from the related table.

4. **Menu and User Information Display**:
   - Create a menu allowing users to select features (menu-bar or button menu).
   - At each screen, the top right corner will display the current user's username and role.

## Technologies Used
- **Programming Language**: C#
- **Framework**: .NET
- **Database**: SQL Server
- **IDE**: Visual Studio

## Team Member Assignment Table
| Team Member | Role | Task Assignments |
|-------------|------------|------------|
| Hà Huy Hoàng | Leader |User Role Management|
| Lê Xuân Phương Nam | Member |CRUD Category|
| Nguyễn Bá Minh Đức | Member |CRUD Account|
| Hoàng	Minh	Đại | Member |Menu and User Information Display|
| Nguyễn Trần Khánh Hà | Member |View Books by Category & Logout|
| Phạm Quốc Quyền | Member |Menu and User Information Display|