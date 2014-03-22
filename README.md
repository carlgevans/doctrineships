Brave Doctrines
===============

An EVE Online ship fitting management and contract validation system.

© 2014 Carl G. Evans and contributors.

https://github.com/DoctrineShips/DoctrineShips

Written in C# MVC.

##1. What is Doctrine Ships?

Doctrine Ships can help to improve the organisation and standardisation of EVE Online fleet doctrines and ensure that alliance/corporation members are flying the correct fits in the shortest amount of time possible.

Members can be sure that they are flying the most up-to-date fitting and that the contracts they are buying have all of the valid components; including ammunition, drones, scripts and repair paste. Invalid contracts are shown in red and can be filtered out.

Move the control of the contracts market back to corporation or alliance management. If sales agents consistently add invalid contracts or attempt to extort members they can be disabled.

Standardise on contract pricing with account-wide profit margin calculations based upon current market data.

Send out fleet pings with a shortened URL for the doctrine you wish members to use. The URL can contain various parameters to filter only valid contracts for the current station and for all ships in a particular doctrine.

Manage contract availability levels and receive alerts when custom thresholds are reached.

##2. Features

Here are some of the features and tools currently available with the system:

* Contract validity checking.
* Contract sorting/filtering with custom URLs for fleet pings.
* Contract availability monitoring and twitter alerts.
* Ship fit imports from EVE XML format (supports bulk import).
* Ship fit details with up-to-date market data.
* Ship fit buying tool.
* Account-level control over settings such as profit percentages.
* Manage sales agents, ship fittings and account permissions/access codes.

##3. Application Design

A diagram of the current application design is available here:

http://i.imgur.com/lydzpqB.png

###3.1 Projects

* DoctrineShips.Data - The DBContext and Entity Framework mappings.
* DoctrineShips.Entities - POCO entities used for internal data structures and by Entity Framework.
* DoctrineShips.Repository - DoctrineShips specific CRUD operations.
* DoctrineShips.Service – The core functionality, linking between the web frontend, EveData, the repository and validation.
* DoctrineShips.Test – Test cases, primarily for the service layer.
* DoctrineShips.Validation – Any type of validation of objects, usually before being written to the DB.
* DoctrineShips.Web – The web frontend.
* EveData – EVE API calls for the data required by Doctrine Ships. Currently uses the XML v2 API.
* EveData.Test  – Test cases specifically for EveData.
* Tools – Reusable methods for conversions, key generation etc.

###3.2 Third-party Libraries

Packages are not included in the repository, so be sure to enable NuGet Package Restore.

* Ninject – IoC / Dependency Injection
* Automapper – Class mapping between layers, primarily between service layer models and view models.
* Linq2Twitter – Used for twitter alerts.
* Grid.Mvc – Filterable and sortable grid views.
* MvcDonutCaching – Improved output caching, allowing for certain page components to be exempt from caching (NavBar etc).
* GenericRepository – A reusable generic repository pattern. Courtesy of http://genericunitofworkandrepositories.codeplex.com/
* Bootstrap
* jQuery

##4. License

Doctrine Ships has been released under the MIT Open Source license. All contributors agree to transfer ownership of their code to Carl G. Evans for release under this license.

###4.1 The MIT License (MIT)

Copyright (c) 2014 DoctrineShips

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.