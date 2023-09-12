using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;

        public event Action<int> OnShoppingCartChanged;

        public ShoppingCartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        //It calls the POST action method within API
        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {

            try
            {
                var response = await httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }

                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public Task<CartItemDto> GetItem(int id)
        {
            throw new NotImplementedException();
        }



        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }

                    return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
                }

            }
            catch (Exception)
            {

                throw;
            }
           

        }



        public async Task<CartItemDto> DeleteItem(int productId, int cartId)
        {
            try
            {
                var response = await this.httpClient.DeleteAsync($"api/ShoppingCart/{productId}/{cartId}");

                if(response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }

                return default(CartItemDto);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                //serialize the dto that needs to be passed to the server into Json format
                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);

                //create object of type string content so it can be passed in the approriate format to the server
                var content = new StringContent(jsonRequest, Encoding.UTF8,"application/json-patch+json");

                var response = await httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.ProductId}/{cartItemQtyUpdateDto.CartId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
            
        }


        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            //check if event has any subscribers
            if(OnShoppingCartChanged != null)
            { 
                //sending appropriate int value to each subscriber by passing in int value to invoke method
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }

    }
}
