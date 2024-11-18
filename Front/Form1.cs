using Front.Services;
using System;
using System.Net.Http;
using System.Windows.Forms;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Front
{
    public partial class Front : Form
    {

        private readonly HttpClient _httpClient;
        private string _authToken;

        public Front()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7164");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string email = textBox4.Text;
                string senha = textBox5.Text;

                var loginModel = new
                {
                    Email = email,
                    Password = senha
                };

                string apiUrl = "/account/login";
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, loginModel);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<RespostaLoginToken>();

                    _authToken = responseData?.Token;

                    if (!string.IsNullOrEmpty(_authToken))
                    {
                        MessageBox.Show("Login bem-sucedido! Token: " + _authToken);
                    }
                    else
                    {
                        MessageBox.Show("Token não encontrado na resposta.");
                    }
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao fazer login: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string email = textBox2.Text;
                string senha = textBox3.Text;
                string nome = textBox1.Text;

                var novoUsuario = new
                {
                    Name = nome,
                    Email = email,
                    Password = senha
                };

                string apiUrl = "/account/signup";
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, novoUsuario);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuário cadastrado com sucesso!");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao cadastrar usuário: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            try
            {
                string apiUrl = "/account/user";

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var usuarios = await response.Content.ReadFromJsonAsync<List<Usuario>>();

                    string usuariosList = string.Join("\n", usuarios.Select(u => $" ({u.Id}) {u.Name} ({u.Email})"));

                    MessageBox.Show("Usuários:\n" + usuariosList);
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao obter usuários: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string apiUrl = "/categories";  // URL da API para listar todas as categorias

                // Adicionando o token ao cabeçalho de autorização
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                // Enviando a requisição GET para obter todas as categorias
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Lendo o conteúdo da resposta como uma lista de NewCategoria
                    var categories = await response.Content.ReadFromJsonAsync<List<NewCategoria>>();

                    // Criando uma string para exibir todas as categorias
                    string categoriesList = string.Join("\n", categories.Select(c => $"{c.Name} - {c.Id}"));

                    // Exibindo as categorias no MessageBox
                    MessageBox.Show("Categorias:\n" + categoriesList);
                }
                else
                {
                    // Se a resposta não for bem-sucedida, exibe a mensagem de erro
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao obter Categorias: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Exibe o erro caso a requisição falhe
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int categoryId = int.Parse(textBox7.Text);  // O ID do produto será inserido nesse TextBox
                string apiUrl = $"/category/{categoryId}";  // URL da API para buscar o produto por ID

                // Adicionando o token ao cabeçalho de autorização
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var category = await response.Content.ReadFromJsonAsync<NewCategoria>();

                    MessageBox.Show($"Categoria: {category.Name}\n ID: {category.Id}\n");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao obter categoria: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtendo os dados do produto dos campos de texto
                var newProduct = new
                {
                    Name = textBox6.Text,
                };

                string apiUrl = "/category";  // URL da API para criar um novo produto

                // Adicionando o token ao cabeçalho de autorização
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, newProduct);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Produto criado com sucesso!");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao criar produto: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int categoryId = int.Parse(textBox7.Text);  // O ID da categoria
                var updatedCategory = new
                {
                    Name = textBox6.Text  // Nome da categoria
                };

                string apiUrl = $"/category/{categoryId}";  // URL da API para atualizar a categoria

                // Adicionando o token ao cabeçalho de autorização
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                // Enviando a requisição PUT para atualizar a categoria
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(apiUrl, updatedCategory);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Categoria atualizada com sucesso!");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao atualizar categoria: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtendo o ID da categoria a ser excluída
                int categoryId = int.Parse(textBox7.Text);  // O ID da categoria

                // URL da API para excluir a categoria
                string apiUrl = $"/category/{categoryId}";  // A URL para excluir a categoria com o ID

                // Adicionando o token ao cabeçalho de autorização
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                // Enviando a requisição DELETE para a API
                HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                // Verificando se a resposta da API foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Categoria excluída com sucesso!");
                }
                else
                {
                    // Caso não tenha sido bem-sucedido, exibe a mensagem de erro
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao excluir categoria: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Exibe o erro caso haja problemas na requisição
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {

            try
            {
                // Pegando o id do produto a partir do TextBox (assumindo que o TextBox se chama 'textBoxProductId')
                if (int.TryParse(textBox8.Text, out int productId))
                {
                    // Verifique se o ID é maior que 0 (opcional, mas é uma boa prática)
                    if (productId <= 0)
                    {
                        MessageBox.Show("Por favor, insira um ID válido.");
                        return;
                    }

                    // Construindo a URL com o ID do produto
                    string apiUrl = $"/product/{productId}";  // Exemplo: "/product/1"

                    // Adicionando o token ao cabeçalho de autorização
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                    // Fazendo a requisição GET
                    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var product = await response.Content.ReadFromJsonAsync<NewProduto>();

                        // Exibindo o produto na interface
                        string productInfo = $"{product.Id}- {product.Name} - {product.Description} - {product.Value} - {product.Category.Id}";
                        MessageBox.Show("Produto encontrado:\n" + productInfo);
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Erro ao obter produto: {errorMessage}");
                    }
                }
                else
                {
                    MessageBox.Show("ID inválido! Por favor, insira um número válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            try
            {
                // URL para pegar todos os produtos
                string apiUrl = "/products";  // Exemplo de rota para obter todos os produtos

                // Adicionando o token de autenticação ao cabeçalho
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                // Fazendo a requisição GET
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);


                if (response.IsSuccessStatusCode)
                {
                    // Lendo a resposta JSON e convertendo para uma lista de produtos
                    var products = await response.Content.ReadFromJsonAsync<List<NewProduto>>();

                    if (products != null && products.Count > 0)
                    {
                        // Exibindo os produtos na interface
                        string productsList = string.Join("\n", products.Select(p =>
                            $"{p.Id}-{p.Name} - {p.Description} - {p.Value} - {p.Category?.Id}"));
                        MessageBox.Show("Produtos encontrados:\n" + productsList);
                    }
                    else
                    {
                        MessageBox.Show("Nenhum produto encontrado.");
                    }
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao obter produtos: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            try
            {
                // Coleta os dados dos campos de texto
                var newProduct = new
                {
                    Name = textBox9.Text,                 // Nome do produto
                    Description = textBox10.Text,   // Descrição do produto
                    Value = decimal.TryParse(textBox11.Text, out var value) ? value : 0,  // Valor do produto
                    CategoryId = int.TryParse(textBox12.Text, out var categoryId) ? categoryId : 0  // ID da categoria
                };

                // Verifica se os dados obrigatórios foram preenchidos
                if (string.IsNullOrWhiteSpace(newProduct.Name) || newProduct.Value <= 0 || newProduct.CategoryId <= 0)
                {
                    MessageBox.Show("Por favor, preencha todos os campos corretamente.");
                    return;
                }

                // Defina a URL de onde a requisição será enviada
                string apiUrl = "/product";  // Supondo que esta seja a URL para criar um novo produto

                // Adiciona o token de autorização no cabeçalho da requisição
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                // Envia a requisição POST com os dados do novo produto
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, newProduct);

                if (response.IsSuccessStatusCode)
                {
                    // Se a resposta for bem-sucedida, exibe uma mensagem de sucesso
                    MessageBox.Show("Produto criado com sucesso!");
                }
                else
                {
                    // Se ocorrer um erro na resposta, exibe a mensagem de erro
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao criar produto: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe a mensagem de erro
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            try
            {
                // Coleta os dados dos campos de texto
                int productId = int.TryParse(textBox8.Text, out var id) ? id : 0;  // ID do produto
                var updatedProduct = new
                {
                    Name = textBox9.Text,                 // Nome do produto
                    Description = textBox10.Text,   // Descrição do produto
                    Value = decimal.TryParse(textBox11.Text, out var value) ? value : 0,  // Valor do produto
                    CategoryId = int.TryParse(textBox12.Text, out var categoryId) ? categoryId : 0  // ID da categoria
                };

                // Verifica se os dados obrigatórios foram preenchidos
                if (productId <= 0 || string.IsNullOrWhiteSpace(updatedProduct.Name) || updatedProduct.Value <= 0 || updatedProduct.CategoryId <= 0)
                {
                    MessageBox.Show("Por favor, preencha todos os campos corretamente.");
                    return;
                }

                // Define a URL da API para atualizar o produto
                string apiUrl = $"/product/{productId}";  // A URL que inclui o ID do produto para atualizar

                // Adiciona o token de autorização no cabeçalho da requisição
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                // Envia a requisição PUT com os dados do produto atualizado
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(apiUrl, updatedProduct);

                if (response.IsSuccessStatusCode)
                {
                    // Se a resposta for bem-sucedida, exibe uma mensagem de sucesso
                    MessageBox.Show("Produto atualizado com sucesso!");
                }
                else
                {
                    // Se ocorrer um erro na resposta, exibe a mensagem de erro
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao atualizar produto: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe a mensagem de erro
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            try
            {
                // Coleta o ID do produto a ser excluído
                int productId = int.TryParse(textBox8.Text, out var id) ? id : 0;  // ID do produto

                // Verifica se o ID é válido
                if (productId <= 0)
                {
                    MessageBox.Show("Por favor, insira um ID de produto válido.");
                    return;
                }

                // Define a URL da API para deletar o produto
                string apiUrl = $"/product/{productId}";  // A URL que inclui o ID do produto para excluir

                // Adiciona o token de autorização no cabeçalho da requisição
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                // Envia a requisição DELETE
                HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Se a resposta for bem-sucedida, exibe uma mensagem de sucesso
                    MessageBox.Show("Produto excluído com sucesso!");
                }
                else
                {
                    // Se ocorrer um erro na resposta, exibe a mensagem de erro
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao excluir produto: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe a mensagem de erro
                MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button14_Click(object sender, EventArgs e)
        {
           
                try
                {
                    // Coleta o ID do usuário a ser excluído
                    int userId = int.TryParse(textBox13.Text, out var id) ? id : 0; // ID do usuário

                    // Verifica se o ID é válido
                    if (userId <= 0)
                    {
                        MessageBox.Show("Por favor, insira um ID de usuário válido.");
                        return;
                    }

                    // Define a URL da API para deletar o usuário
                    string apiUrl = $"/account/user/{userId}"; // A URL que inclui o ID do usuário para excluir

                    // Adiciona o token de autorização no cabeçalho da requisição
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                    // Envia a requisição DELETE
                    HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Se a resposta for bem-sucedida, exibe uma mensagem de sucesso
                        MessageBox.Show("Usuário excluído com sucesso!");
                    }
                    else
                    {
                        // Se ocorrer um erro na resposta, exibe a mensagem de erro
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Erro ao excluir usuário: {errorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    // Em caso de erro, exibe a mensagem de erro
                    MessageBox.Show($"Erro ao acessar a API: {ex.Message}");
                }
            

        }

        private void Registro_Click(object sender, EventArgs e)
        {

        }

        private void Front_Load(object sender, EventArgs e)
        {

        }
    }
}
