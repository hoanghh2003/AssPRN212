Feature: Add Report Functionality
  In order to manage employee reports
  As an admin user
  I want to be able to add new reports with specific criteria and display fields

  Scenario Outline: Add report with different inputs
    Given the user is on the "Add Report" page
    When the user enters "<report_name>" as the Report Name
    And selects "<selection_criteria>" as the Selection Criteria
    And selects "<display_field_group>" as the Display Field Group
    And chooses "<display_fields>" as Display Fields
    And clicks the "+" button to confirm the fields
    And clicks "Save"
    Then the system should display the message "<expected_message>"

    Examples:
      | report_name        | selection_criteria           | display_field_group | display_fields                       | expected_message              |
      | Employee Report 1  | Include Current Employees    | Personal            | Employee First Name, Date of Birth  | Success                       |
      | Employee Report 2  | Include Terminated Employees | Personal            | Employee Last Name                  | Success                       |
      | Report Without Name|                              | Personal            | Employee First Name                 | Required below Report Name    |
      | Report 3           | Include Current Employees    |                     |                                    | At least one display field    |
      | Long Report Name   | Include Current Employees    | Personal            | Employee First Name                | Should not exceed 250 characters |
      | Duplicate Report   | Include Current Employees    | Personal            | Employee First Name                | Already exists                |
