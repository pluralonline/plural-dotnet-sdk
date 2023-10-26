namespace Pinelabs_Example;
using System.Text.Json;
using PineLabsSdk;

public class PinelabsExample
{
    private Payment _payment = new Payment("106600", "bcf441be-411b-46a1-aa88-c6e852a7d68c", "9A7282D0556544C59AFE8EC92F5C85F6", true);

    private EMI _emi = new EMI("106600", "bcf441be-411b-46a1-aa88-c6e852a7d68c", true);
    
    public async Task InitPinelabs(String[] args)
    {
        switch (args.Length > 0 ? args[0] : "")
        {
            case "--create":
                await this.CreateOrder();
                break;
            case "--fetch":
                await this.FetchOrder();
                break;
            case "--emi":
                await this.EmiCalculator();
                break;
            case "--hash":
                VerifyHash();
                break;
            case "--help":
                Console.WriteLine(
                    "Incorrect Or Missing Flag. Please pass one of \n [ \n --create: For creating a payment url,\n --fetch: For fetching transaction details,\n --emi: For fetching emi details for a product,\n --hash: For executing hash verification method on a sample response \n ]");
                break;
            default:
                Console.WriteLine(
                    "Incorrect Or Missing Flag. Please pass one of \n [ \n --create: For creating a payment url,\n --fetch: For fetching transaction details,\n --emi: For fetching emi details for a product,\n --hash: For executing hash verification method on a sample response \n ]");
                break;
        }
    }

    private async Task CreateOrder()
    {
        var transactionData = new txn_data();

        transactionData.txn_id = new Random().Next(999, 999999999).ToString();
        transactionData.amount_in_paisa = "10000";
        transactionData.callback = "https://httpbin.org/post";

        var customerData = new customer_data();
        customerData.email_id = "test@pinelabs.in";
        customerData.first_name = "John";
        customerData.last_name = "Doe";
        customerData.mobile_no = "92842714583";
        customerData.customer_id = "43243242423242432";

        var paymentModes = new payment_mode();

        paymentModes.cards = true;
        paymentModes.bnpl = true;

        // Create Order
        var response = await this._payment.Create(transactionData, customerData, new billing_data(),
            new shipping_data(),
            new udf_data(),
            paymentModes);

        Console.WriteLine(JsonSerializer.Serialize(response));
    }

    private async Task FetchOrder()
    {
        // Fetch Order
        var response = await this._payment.Fetch("650acb67d3752", 3);
        Console.WriteLine(JsonSerializer.Serialize(response));
    }

    private async Task EmiCalculator()
    {
        // Emi Calculator
        var product1 = new product_details();
        product1.product_code = "testSKU1";
        product1.product_amount = 500000;

        product_details[] details = { product1 };
        var response = await this._emi.CalculateEmi("500000", details);

        Console.WriteLine(JsonSerializer.Serialize(response));
    }

    private static void VerifyHash()
    {
        // Verify Hash
        var isVerified = Hash.VerifyHash("D640CFF0FCB8D42B74B1AFD19D97A375DAF174CCBE9555E40CC6236964928896", new
        {
            ppc_MerchantID = "106600",
            ppc_MerchantAccessCode = "bcf441be-411b-46a1-aa88-c6e852a7d68c",
            ppc_PinePGTxnStatus = "7",
            ppc_TransactionCompletionDateTime = "20/09/2023 04:07:52 PM",
            ppc_UniqueMerchantTxnID = "650acb67d3752",
            ppc_Amount = "1000",
            ppc_TxnResponseCode = "1",
            ppc_TxnResponseMessage = "SUCCESS",
            ppc_PinePGTransactionID = "12069839",
            ppc_CapturedAmount = "1000",
            ppc_RefundedAmount = "0",
            ppc_AcquirerName = "BILLDESK",
            ppc_PaymentMode = "3",
            ppc_Parent_TxnStatus = "4",
            ppc_ParentTxnResponseCode = "1",
            ppc_ParentTxnResponseMessage = "SUCCESS",
            ppc_CustomerMobile = "7737291210",
            ppc_UdfField1 = "",
            ppc_UdfField2 = "",
            ppc_UdfField3 = "",
            ppc_UdfField4 = "",
            ppc_AcquirerResponseCode = "0300",
            ppc_AcquirerResponseMessage = "NA"
        }, "9A7282D0556544C59AFE8EC92F5C85F6");
        Console.WriteLine(isVerified);
    }
}