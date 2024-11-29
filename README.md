Approach 1.
    According to the input currency, update the endpoint with the respecieve Currency at the end to get the output currency rate Which doesnt need much of a calculation.
    Giving the base currency get the exchange rate of the output curerency and just multiply to the amount to get the rate. 
Approach 2.(Implemented)
    Always get the currency exchanges rates for related to USD and derive the amount for the output Currency. could be a minor change from Approch 1. because of rounding.
    
According to the Response, It gives the next update time. If it means that the rate wont change till the next update time, The CurrencyExchange rates can be saved in Redis and can be refreshed. 