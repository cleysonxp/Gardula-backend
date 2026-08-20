using Gardula.Domain.Entities.Finance;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Infrastructure.Persistence.Seeds;

public static class CategorySeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTimeOffset(
            2026,
            8,
            20,
            0,
            0,
            0,
            TimeSpan.Zero);

        modelBuilder.Entity<Category>().HasData(

            // ============================================================
            // DESPESAS
            // ============================================================

            // Moradia
            new
            {
                Id = 1,
                UserId = (int?)null,
                Name = "Moradia",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 2,
                UserId = (int?)null,
                Name = "Aluguel",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)1,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 3,
                UserId = (int?)null,
                Name = "Condomínio",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)1,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 4,
                UserId = (int?)null,
                Name = "Contas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)1,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 5,
                UserId = (int?)null,
                Name = "Manutenção",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)1,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 6,
                UserId = (int?)null,
                Name = "Móveis e eletrodomésticos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)1,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Alimentação
            new
            {
                Id = 7,
                UserId = (int?)null,
                Name = "Alimentação",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 8,
                UserId = (int?)null,
                Name = "Mercado",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)7,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 9,
                UserId = (int?)null,
                Name = "Restaurantes",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)7,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 10,
                UserId = (int?)null,
                Name = "Delivery",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)7,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 11,
                UserId = (int?)null,
                Name = "Lanches",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)7,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Transporte
            new
            {
                Id = 12,
                UserId = (int?)null,
                Name = "Transporte",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 13,
                UserId = (int?)null,
                Name = "Combustível",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)12,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 14,
                UserId = (int?)null,
                Name = "Transporte por aplicativo",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)12,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 15,
                UserId = (int?)null,
                Name = "Transporte público",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)12,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 16,
                UserId = (int?)null,
                Name = "Manutenção",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)12,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 17,
                UserId = (int?)null,
                Name = "Estacionamento",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)12,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 18,
                UserId = (int?)null,
                Name = "Documentação e impostos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)12,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Saúde
            new
            {
                Id = 19,
                UserId = (int?)null,
                Name = "Saúde",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 20,
                UserId = (int?)null,
                Name = "Farmácia",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)19,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 21,
                UserId = (int?)null,
                Name = "Consultas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)19,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 22,
                UserId = (int?)null,
                Name = "Exames",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)19,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 23,
                UserId = (int?)null,
                Name = "Plano de saúde",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)19,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 24,
                UserId = (int?)null,
                Name = "Odontologia",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)19,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Lazer
            new
            {
                Id = 25,
                UserId = (int?)null,
                Name = "Lazer",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 26,
                UserId = (int?)null,
                Name = "Cinema",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)25,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 27,
                UserId = (int?)null,
                Name = "Jogos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)25,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 28,
                UserId = (int?)null,
                Name = "Passeios",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)25,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 29,
                UserId = (int?)null,
                Name = "Eventos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)25,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 30,
                UserId = (int?)null,
                Name = "Hobbies",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)25,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Compras
            new
            {
                Id = 31,
                UserId = (int?)null,
                Name = "Compras",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 32,
                UserId = (int?)null,
                Name = "Roupas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)31,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 33,
                UserId = (int?)null,
                Name = "Eletrônicos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)31,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 34,
                UserId = (int?)null,
                Name = "Casa",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)31,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 35,
                UserId = (int?)null,
                Name = "Outros",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)31,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Assinaturas
            new
            {
                Id = 36,
                UserId = (int?)null,
                Name = "Assinaturas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 37,
                UserId = (int?)null,
                Name = "Streaming",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)36,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 38,
                UserId = (int?)null,
                Name = "Software",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)36,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 39,
                UserId = (int?)null,
                Name = "Serviços digitais",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)36,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Educação
            new
            {
                Id = 40,
                UserId = (int?)null,
                Name = "Educação",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 41,
                UserId = (int?)null,
                Name = "Cursos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)40,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 42,
                UserId = (int?)null,
                Name = "Livros",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)40,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 43,
                UserId = (int?)null,
                Name = "Material",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)40,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 44,
                UserId = (int?)null,
                Name = "Mensalidades",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)40,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Pets
            new
            {
                Id = 45,
                UserId = (int?)null,
                Name = "Pets",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 46,
                UserId = (int?)null,
                Name = "Alimentação",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)45,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 47,
                UserId = (int?)null,
                Name = "Veterinário",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)45,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 48,
                UserId = (int?)null,
                Name = "Higiene",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)45,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Viagens
            new
            {
                Id = 49,
                UserId = (int?)null,
                Name = "Viagens",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 50,
                UserId = (int?)null,
                Name = "Passagens",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)49,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 51,
                UserId = (int?)null,
                Name = "Hospedagem",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)49,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 52,
                UserId = (int?)null,
                Name = "Alimentação",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)49,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 53,
                UserId = (int?)null,
                Name = "Passeios",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)49,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 54,
                UserId = (int?)null,
                Name = "Transporte",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)49,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Trabalho
            new
            {
                Id = 55,
                UserId = (int?)null,
                Name = "Trabalho",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 56,
                UserId = (int?)null,
                Name = "Equipamentos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)55,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 57,
                UserId = (int?)null,
                Name = "Cursos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)55,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 58,
                UserId = (int?)null,
                Name = "Ferramentas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)55,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Impostos e taxas
            new
            {
                Id = 59,
                UserId = (int?)null,
                Name = "Impostos e taxas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 60,
                UserId = (int?)null,
                Name = "Impostos",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)59,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 61,
                UserId = (int?)null,
                Name = "Tarifas bancárias",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)59,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 62,
                UserId = (int?)null,
                Name = "Multas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)59,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 63,
                UserId = (int?)null,
                Name = "Taxas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)59,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Pessoal
            new
            {
                Id = 64,
                UserId = (int?)null,
                Name = "Pessoal",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 65,
                UserId = (int?)null,
                Name = "Cuidados pessoais",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)64,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 66,
                UserId = (int?)null,
                Name = "Roupas",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)64,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 67,
                UserId = (int?)null,
                Name = "Academia",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)64,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Emergência
            new
            {
                Id = 68,
                UserId = (int?)null,
                Name = "Emergência",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 69,
                UserId = (int?)null,
                Name = "Saúde",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)68,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 70,
                UserId = (int?)null,
                Name = "Casa",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)68,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 71,
                UserId = (int?)null,
                Name = "Transporte",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)68,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 72,
                UserId = (int?)null,
                Name = "Outros",
                Type = CategoryType.Expense,
                ParentCategoryId = (int?)68,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // ============================================================
            // RECEITAS
            // ============================================================

            // Salário
            new
            {
                Id = 73,
                UserId = (int?)null,
                Name = "Salário",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 74,
                UserId = (int?)null,
                Name = "Salário",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)73,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 75,
                UserId = (int?)null,
                Name = "13º",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)73,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 76,
                UserId = (int?)null,
                Name = "Férias",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)73,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 77,
                UserId = (int?)null,
                Name = "Bonificação",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)73,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Freelance
            new
            {
                Id = 78,
                UserId = (int?)null,
                Name = "Freelance",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 79,
                UserId = (int?)null,
                Name = "Desenvolvimento",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)78,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 80,
                UserId = (int?)null,
                Name = "Consultoria",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)78,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 81,
                UserId = (int?)null,
                Name = "Outros",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)78,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Investimentos
            new
            {
                Id = 82,
                UserId = (int?)null,
                Name = "Investimentos",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 83,
                UserId = (int?)null,
                Name = "Dividendos",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)82,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 84,
                UserId = (int?)null,
                Name = "Juros",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)82,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 85,
                UserId = (int?)null,
                Name = "Rendimentos",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)82,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Reembolsos
            new
            {
                Id = 86,
                UserId = (int?)null,
                Name = "Reembolsos",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 87,
                UserId = (int?)null,
                Name = "Trabalho",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)86,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 88,
                UserId = (int?)null,
                Name = "Compras",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)86,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 89,
                UserId = (int?)null,
                Name = "Outros",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)86,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Vendas
            new
            {
                Id = 90,
                UserId = (int?)null,
                Name = "Vendas",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 91,
                UserId = (int?)null,
                Name = "Produtos",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)90,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 92,
                UserId = (int?)null,
                Name = "Serviços",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)90,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 93,
                UserId = (int?)null,
                Name = "Outros",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)90,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },

            // Outros
            new
            {
                Id = 94,
                UserId = (int?)null,
                Name = "Outros",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)null,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 95,
                UserId = (int?)null,
                Name = "Presentes",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)94,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new
            {
                Id = 96,
                UserId = (int?)null,
                Name = "Recebimentos diversos",
                Type = CategoryType.Income,
                ParentCategoryId = (int?)94,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );
    }
}