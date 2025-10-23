using AbacatePay;
using AbacatePay.Models.Coupon;

namespace AbacatePay.Examples;

/// <summary>
/// Exemplo de uso dos Cupons de Desconto com AbacatePay SDK
/// </summary>
public class CouponExample
{
    public static async Task RunExample()
    {
        // Configuração do cliente
        var client = new AbacatePayClient("sua_api_key_aqui", sandbox: true);

        try
        {
            // 1. Criar um cupom de desconto percentual
            Console.WriteLine("=== Criando Cupom de Desconto Percentual ===");
            
            var percentageCoupon = new CouponRequest
            {
                Data = new CouponData
                {
                    Code = "DESCONTO20",
                    Notes = "Cupom de desconto para meu público",
                    MaxRedeems = 10,
                    DiscountKind = DiscountKind.PERCENTAGE,
                    Discount = 20, // 20% de desconto
                    Metadata = new Dictionary<string, object>
                    {
                        { "campaign", "black_friday" },
                        { "target_audience", "new_customers" }
                    }
                }
            };

            var createdPercentageCoupon = await client.CreateCouponAsync(percentageCoupon);
            Console.WriteLine($"✅ Cupom percentual criado: {createdPercentageCoupon.Id}");
            Console.WriteLine($"   Status: {createdPercentageCoupon.Status}");
            Console.WriteLine($"   Desconto: {createdPercentageCoupon.Discount}%");
            Console.WriteLine($"   Máximo de resgates: {createdPercentageCoupon.MaxRedeems}");

            // 2. Criar um cupom de desconto fixo
            Console.WriteLine("\n=== Criando Cupom de Desconto Fixo ===");
            
            var fixedCoupon = new CouponRequest
            {
                Data = new CouponData
                {
                    Code = "DESCONTO5REAIS",
                    Notes = "Cupom de desconto fixo de R$ 5,00",
                    MaxRedeems = 100,
                    DiscountKind = DiscountKind.FIXED,
                    Discount = 500, // R$ 5,00 de desconto fixo
                    Metadata = new Dictionary<string, object>
                    {
                        { "campaign", "welcome_bonus" },
                        { "min_order_value", 1000 }
                    }
                }
            };

            var createdFixedCoupon = await client.CreateCouponAsync(fixedCoupon);
            Console.WriteLine($"✅ Cupom fixo criado: {createdFixedCoupon.Id}");
            Console.WriteLine($"   Status: {createdFixedCoupon.Status}");
            Console.WriteLine($"   Desconto: R$ {createdFixedCoupon.Discount / 100:F2}");
            Console.WriteLine($"   Máximo de resgates: {createdFixedCoupon.MaxRedeems}");

            // 3. Criar um cupom ilimitado
            Console.WriteLine("\n=== Criando Cupom Ilimitado ===");
            
            var unlimitedCoupon = new CouponRequest
            {
                Data = new CouponData
                {
                    Code = "DESCONTOILIMITADO",
                    Notes = "Cupom de desconto ilimitado",
                    MaxRedeems = -1, // Ilimitado
                    DiscountKind = DiscountKind.PERCENTAGE,
                    Discount = 10, // 10% de desconto
                    Metadata = new Dictionary<string, object>
                    {
                        { "campaign", "loyalty_program" },
                        { "vip_customers", true }
                    }
                }
            };

            var createdUnlimitedCoupon = await client.CreateCouponAsync(unlimitedCoupon);
            Console.WriteLine($"✅ Cupom ilimitado criado: {createdUnlimitedCoupon.Id}");
            Console.WriteLine($"   Status: {createdUnlimitedCoupon.Status}");
            Console.WriteLine($"   Desconto: {createdUnlimitedCoupon.Discount}%");
            Console.WriteLine($"   Máximo de resgates: {(createdUnlimitedCoupon.MaxRedeems == -1 ? "Ilimitado" : createdUnlimitedCoupon.MaxRedeems.ToString())}");

            // 4. Listar todos os cupons
            Console.WriteLine("\n=== Listando Todos os Cupons ===");
            
            var allCoupons = await client.ListCouponsAsync();
            Console.WriteLine($"📋 Total de cupons encontrados: {allCoupons.Count}");
            
            foreach (var coupon in allCoupons)
            {
                Console.WriteLine($"   • {coupon.Id}");
                Console.WriteLine($"     Status: {coupon.Status}");
                Console.WriteLine($"     Desconto: {(coupon.DiscountKind == DiscountKind.PERCENTAGE ? $"{coupon.Discount}%" : $"R$ {coupon.Discount / 100:F2}")}");
                Console.WriteLine($"     Resgates: {coupon.RedeemsCount}/{coupon.MaxRedeems}");
                Console.WriteLine($"     Criado em: {coupon.CreatedAt:dd/MM/yyyy HH:mm}");
                Console.WriteLine();
            }

        }
        catch (AbacatePayException ex)
        {
            Console.WriteLine($"❌ Erro da API AbacatePay: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
        }
        finally
        {
            // Dispose do cliente
            client.Dispose();
        }
    }
}
