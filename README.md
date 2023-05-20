# Online-Store
<h2>Descripiton :</h2>
<p> A RESTful API for an online store that mimics Amazon (website).</p>
<br/>
<h2>Technologies used :</h2>
1. C#  <br>
2. ASP.NET Web API .NET6.<br>
3. Redis. <br>
4. Supabase storage (cloud storage) to upload images. <br>
5. Swagger UI (Documenting the application).  <br>
6. Entity Framework Core (Database First Approach). <br>
7. microsoft sql server  2019.<br>
8. Docker for running Redis.<br>
9. Postman.


<h2>Running the API Locally</h2>
- To run the Online-Store Web API, you must have the following prerequisites installed:
<ul>
  <li>.NET Core SDK version 6 </li>
  <li>MS SQL Server or SQL Server Express</li>
  <li>Docker for running Redis</li>
  <li>A text editor or integrated development environment (IDE) such as Visual Studio Code</li>
</ul>
- To get started, follow these steps:
<ol>
  <li>Clone the repository to your local machine.</li>
  <li>Open the solution file online-store.sln in your preferred IDE or text editor.</li>
  <li>Update the database connection string in the appsettings.json file with your MS SQL Server connection string.</li>
  <li>Run the application using your IDE Or open the project on your terminal then run this command "dotnet run".</li>
  
</ol>
<h2>Authentication and Authorizationy</h2>
<p dir="auto">
"The Online-Store Web API uses  JWT (JSON Web Tokens) for authentication and authorization.
To access the API endpoints, you must obtain a valid JWT token by logging in with valid credentials. When a user logs in, the access token and refresh token are generated and returned to the client. This access token should be included in the Authorization header of subsequent requests. The API provides the following endpoints for authentication:"
</p>
<ul> 
<li>/api/Authentication/register-as-a-customer - Allows users to register for a new account as a customer</li>
<li>/api/Authentication/register-as-a-vendor- Allows users to register for a new account as a vendor</li>
<li>/api/Authentication/register-as-an-admin- Admin registration</li>
<li>/api/Authentication/login - Allows a user to log in to their account and receive a JWT token and refresh token.</li>
<li>/api/Authentication/refresh-token - Allows a user to refresh token and receive a JWT token and refresh token.</li>
<li>/api/Authentication/revoke-token - Allows a user to revoke the refresh token.</li>
</ul>
<p>
  The API also includes authorization checks on certain endpoints, ensuring that only authenticated users with the appropriate role can perform certain actions. The available roles are:
</p>
<ul>
  <li>Admin: Has full access to all endpoints.</li>
  <li>vendor : Has access to all products and categories endpoints.</li>
  <li>customer : Has limited access to only the GET endpoints for products,categories,orders and payment.</li>
</ul>
<h2>Redis Usage</h2>
<p>
  - Redis plays a crucial role in the online store Api by providing efficient data storage and caching capabilities. By leveraging Redis's in-memory nature, we significantly enhance the performance and responsiveness of our application. 
</p>
<p>
  - I utilize Redis to improve performance for most used endpoints like endpoints that Get products.
</p>
<p>- This project uses Redis to store data for the following keys:</p>
<ul>
  <li><code>Products:category:{categoryId}</code> This key is used to store products belonging to a specific category with the `categoryId`. For example, `products:category:123` represents the products associated with category 123. Storing products by category enables efficient retrieval and filtering of products based on their categories.</li>
  <li>
    <code>products:{page_number}:{page_size}</code> This key stores paginated product data for a given page number and page size. For instance, `products:2:20` represents the products displayed on the second page with 20 products per page. Utilizing this key structure allows us to fetch products in chunks and implement pagination efficiently.
  </li>
  <li>
     <code>products</code> This key represents a general key for storing all products. It is used to cache all products. Storing products in Redis allows for faster retrieval compared to querying a database, leading to improved performance and responsiveness.
  </li>
  <li>
    <code>products:search:{search_query}</code> This key is used for storing products related to a specific search query. For example, `products:search:laptop` represents the products found through a search query for "laptop." This key helps speed up search functionality by storing and retrieving relevant products based on user search queries.
  </li>
    
  
</ul>

<p>
  - To connect the online store with Redis, modify the relevant configuration file <code>appsettings.json</code> to include the connection details for Redis. 
  <code>"RedisConnectionString" :  "localhost:6379"</code>
</p>
<h2>Deployment</h2>
<h4>Details coming soon</h4>
<h2>Packages</h2>
<pre class="notranslate" style="position: relative;"><code>  AutoMapper
  AutoMapper.Extensions.Microsoft.DependencyInjection
  Microsoft.AspNetCore.Authentication.JwtBearer
  Microsoft.AspNetCore.Mvc.NewtonsoftJson
  Microsoft.EntityFrameworkCore
  Microsoft.EntityFrameworkCore.SqlServer
  Microsoft.EntityFrameworkCore.Tools
  Microsoft.Extensions.Caching.StackExchangeRedis
  Swashbuckle.AspNetCore 
  Swashbuckle.AspNetCore.Filters
  Serilog
  Serilog.AspNetCore
  supabase-csharp
  supabase-storage-csharp
</code><div class="open_grepper_editor" title="Edit &amp; Save To Grepper"></div></pre>
