using iTextSharp.text.pdf;
using iTextSharp.text;
using Newtonsoft.Json;

public class Employee
{
    [JsonProperty("ID")]
    public int ID { get; set; }
    [JsonProperty("First Name")]
    public string FirstName { get; set; } = string.Empty;
    [JsonProperty("Last Name")]
    public string LastName { get; set; } = string.Empty;
    [JsonProperty("Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    




    //public static void LoadDataFromJson(string jsonContent)
    //{
    //    try
    //    {
    //        // קוד הטעינה מקובץ JSON

    //        // פעולות טעינה ועיבוד של נתוני ה-JSON
    //        List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(jsonContent);

    //        // כאן תוכל לבצע פעולות נוספות על נתוני ה-JSON, כמו להציגם בממשק המשתמש או לעבדם בכל דרך אחרת

    //        // לדוגמה, ניתן להציג את הנתונים בטבלה
    //        DisplayDataInTable(employees);
    //    }
    //    catch (Exception ex)
    //    {
    //        // טיפול בשגיאות
    //        Console.WriteLine("Error: " + ex.Message);
    //    }
    //}

    //private static void DisplayDataInTable(List<Employee> employees)
    //{

    //}

}
