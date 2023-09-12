using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
       
        //Method to call GetRequest to get all products and its categories
        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {       //translates returned Json data into appropriate object type
                //var products = await this.httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
                //return products;

                var response = await this.httpClient.GetAsync("api/Product");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }


        //Method to call GetRequest to get product and its category
        public async Task<ProductDto> GetItem(int id)
        {
            try
            {                    //Doesnt convert the returned Json data to object type. It needs to be done seperately
                var response = await httpClient.GetAsync($"api/Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }

                    return await response.Content.ReadFromJsonAsync<ProductDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {
                //Log exception 
                throw;
            }
        }
     
    }
}
