# WooliesChallenge

### Compile and Run the solution
Open the solution in VS 2019. Compile the solution  and set the project "WooliesChallenge.Functions" as the "startup project" and run. This will display the local URLs that can be used to run the functions locally.

### Hosted functions 
The function app is hosted in Azure cloud and can be access with the below mentioned urls.
              
### Exercise 1 
This excercise returns a User object with a name and the token.

 `<link>` : https://woolieschallengefunctions.azurewebsites.net/api/answers/user?
 
### Exercise 2
This excercise returns the sorted products with required sort options by fetching the products from a Resource API.
The sort option "Recommended" calls another Resource API and sorts with popularity.

`<link>` : https://woolieschallengefunctions.azurewebsites.net/api/sort?

Note: The given woolies link to test fails for "Recommended" option but passes for all others.

### Exercise 3
This excercise calls another resource to calculate the trolley total.

`<link>` : https://woolieschallengefunctions.azurewebsites.net/api/trolleyTotal?

## Technical Details
- Solution is developed using .NET Core 3.1 and Azure functions v3
- Http Triggered Azure functions are created.
- Unit test: MSTest and Moq is used as mocking framework.
- Http calls to external resources are made using IHttpClientFactory for resilient implementation.

  
  
