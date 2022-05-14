# HITS.Demo.WeakEvents
This solution demonstrates the use of the Microsoft WeakReferenceMessenger class in a Blazor Server-Side application.
Using the standard out-of-the-box Blazor application, an event is raised everytime the counter value is incremented.
The event is handled by the MainLayout class which displays the count in the top row of the page.
Session event separation is accomplished by assigning a session ID to each session which is passed with each event publication.
Test the session separation by opening multiple browser windows to ensure each counter increments independently.

Other features of this application...

Visual Studio 2022 Community Edition (c#)

No jscript

No MVC

Session State

SQLite Logging

Class Isolation

Custom Data Grid

Country Exclusion via IP Location Lookup


In the OnInitializedAsync event we capture the requesters IP address.
I use https://ipapi.com/ to get the location associated with the IP address.
Go to https://ipapi.com/ to get a free API key.

mikehillman.net

