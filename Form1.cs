using iTextSharp.text.pdf;
using iTextSharp.text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Forms;




namespace JsonPdf6
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog; // הגדרת המשתנה ברמת הטופס

        private string logoPath = ""; // הוספת משתנה גלובלי עבור 'logoPath'
        private string filePath = ""; // הגדרת המשתנה ברמת הטופס כמחרוזת ריקה
        private string userText = ""; // הגדרת משתנה גלובלי לשמירת הטקסט שהמשתמש מזין

        private List<Employee> employees; // הגדרת המשתנה כחלק מהמחלקה

        private PdfWriter writer;

        private PdfFooter pdfFooter; // הוספת ההגדרה של המשתנה pdfFooter



        private TextBox txtID;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtPhoneNumber;
        private TextBox txtEmail;





        public Form1()
        {
            InitializeComponent();


        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // יצירת והוספת הטקסטבוקסים לממשק המשתמש (UI)
            txtID = new TextBox();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtPhoneNumber = new TextBox();
            txtEmail = new TextBox();

        }
                    //לוגו  
        private void BtnUploadLogo_Click(object sender, EventArgs e)
        {
            using (openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
                openFileDialog.Title = "Select Logo Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    logoPath = openFileDialog.FileName; // שמירת הנתיב של הלוגו הנבחר
                }
            }
        }







                      //  JSON
        private void UploadJSON_Click(object sender, EventArgs e)
        {
            // יצירת אובייקט OpenFileDialog
            openFileDialog = new OpenFileDialog();

            // הגדרות ה-OpenFileDialog
            openFileDialog.Filter = "JSON Files (*.json)|*.json";
            openFileDialog.Title = "Select JSON File";

            // בדיקה אם המשתמש בחר קובץ ולחץ על אישור
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName; // שמירת הנתיב של קובץ ה-JSON הנבחר

                // תצוגת הנתונים באמצעות טבלה PDF-Form שלך
                LoadJsonData(filePath);
                DisplayPDFData();
            }
            
        }


        private string GetPdfFilePath(string filePath)
        {
            // בדיקה האם נבחר קובץ JSON ולוגו כדי לייצר קובץ PDF
            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(logoPath))
            {
                MessageBox.Show("אנא בחר קובץ JSON ולוגו קודם כדי ליצור קובץ PDF.");
                return null; // או תחזיר מחרוזת ריקה או ערך שאתה רוצה להשתמש בו בהמשך
            }

            // ייבוצע פה הגנרציה של נתיב הקובץ PDF על סמך נתיב הקובץ JSON
            return Path.ChangeExtension(filePath, ".pdf");
        }

                     //   PDF
        private void DisplayPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // בדיקה שנבחר קובץ JSON קודם
                if (employees == null || employees.Count == 0)
                {
                    MessageBox.Show("אנא העלה תחילה קובץ JSON.");
                    return;
                }

                //// תצוגת הנתונים באמצעות טבלה PDF-Form שלך
                //string pdfFilePath = GetPdfFilePath(filePath); // קריאה לפונקציה וקבלת ערך המשתנה pdfFilePath
                // תצוגת הנתונים באמצעות טבלה PDF-Form שלך
                 string pdfFilePath = DisplayPDFData(); // קריאה לפונקציה וקבלת ערך המשתנה pdfFilePath


                if (pdfFilePath != null)
                {
                    // תיבת טקסט לקבלת הטקסט מהמשתמש
                    using (var inputForm = new InputForm())
                    {
                        if (inputForm.ShowDialog() == DialogResult.OK)
                        {
                            string footerText = inputForm.UserText; // קבלת הטקסט מהמשתמש
                            UpdatePdfFooter(footerText, pdfFilePath); // עדכון הטקסט בתחתית הדף
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // טיפול בשגיאה בטעינת הנתונים
                MessageBox.Show($"אירעה שגיאה בעת יצירת PDF:{Environment.NewLine}{ex.ToString()}");
            }
        }





        // פונקציה לטעינת הנתונים מקובץ JSON
        private void LoadJsonData(string filePath)
        {
            try
            {
                // בדיקת קיום הקובץ בנתיב
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("קובץ ה-JSON לא נמצא בנתיב הנתון.");
                    return;
                }

                // קריאת הנתונים מהקובץ
                string jsonContent = File.ReadAllText(filePath);

                // טעינת הנתונים לתוך רשימת אובייקטי Employee
                employees = JsonConvert.DeserializeObject<List<Employee>>(jsonContent);

                //// תצוגת הנתונים באמצעות טבלה PDF-Form שלך
                //DisplayDataPDF();
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה בטעינת הנתונים
                MessageBox.Show($"אירעה שגיאה בטעינת הנתונים:{Environment.NewLine}{ex.ToString()}");
            }
        }


        private string DisplayPDFData()
        {
            // בדיקה האם קיימים נתונים לתצוגה
            if (employees == null || employees.Count == 0)
            {
                MessageBox.Show("אין נתונים להצגה.");
                return null; // או כל ערך אחר שמתאים לך
            }

            // בדיקה האם נבחר קובץ JSON ולוגו כדי לייצר קובץ PDF
            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(logoPath))
            {
                MessageBox.Show("אנא בחר קובץ JSON ולוגו קודם כדי ליצור קובץ PDF.");
                return null; // או כל ערך אחר שמתאים לך
            }

            // ייבוצע פה הגנרציה של נתיב הקובץ PDF על סמך נתיב הקובץ JSON
            string pdfFilePath = Path.ChangeExtension(filePath, ".pdf");

            using (var inputForm = new InputForm())
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    string footerText = inputForm.UserText; // קבלת הטקסט מהמשתמש

                    // עדכון הטקסט בתחתית הדף
                    UpdatePdfFooter(footerText, pdfFilePath);

                    // יצירת המסמך PDF עם הנתונים הנוכחיים
                    PdfGenerator.GeneratePdf(employees, pdfFilePath, logoPath, footerText);

                    // הפונקציה מחזירה את הנתיב של קובץ ה-PDF
                    return pdfFilePath;
                }
                else
                {
                    // אם המשתמש לא בחר ליצור קובץ PDF, נחזיר ערך ריק
                    return string.Empty;
                }
            }

        }






        private List<Employee> GetDataToDisplay(string jsonFilePath)
        {
            List<Employee> employees = new List<Employee>();

            // בדיקה האם נתיב הקובץ JSON תקין
            if (string.IsNullOrEmpty(jsonFilePath))
            {
                MessageBox.Show("נתיב הקובץ JSON אינו תקין.");
                return employees;
            }

            // בדיקה האם קובץ ה-JSON קיים בנתיב
            if (!File.Exists(jsonFilePath))
            {
                MessageBox.Show("קובץ ה-JSON לא נמצא בנתיב הנתון.");
                return employees;
            }

            try
            {
                // קריאת הנתונים מהקובץ
                string jsonContent = File.ReadAllText(jsonFilePath);

                // המרת הנתונים לרשימת אובייקטי Employee
                employees = JsonConvert.DeserializeObject<List<Employee>>(jsonContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"אירעה שגיאה בטעינת הנתונים מהקובץ JSON:{Environment.NewLine}{ex.Message}");
            }

            return employees;
        }


        // פונקציה לעדכון הטקסט בתחתית הדף
        private void UpdatePdfFooter(string userText, string pdfFilePath)
        {
            if (!string.IsNullOrWhiteSpace(userText))
            {
                if (pdfFooter == null) // בדיקה אם עוד לא יצרנו את האובייקט
                {
                    pdfFooter = new PdfFooter(userText); // יצירת אובייקט PdfFooter עם הטקסט של תחתית הדף
                }
                else
                {
                    pdfFooter.FooterText = userText; // עדכון ישיר של ערך התחתית
                }

                PdfGenerator.GeneratePdf(employees, pdfFilePath, logoPath, userText); // עדכון הטקסט בתחתית הדף וייצור המסמך
            }
        }






        private void ExitButton_Click(object sender, EventArgs e)
        {
            // יציאה מהאפליקציה
            Application.Exit();
        }


    }

    public class InputForm : Form
    {
        private TextBox textBox;
        private Button okButton;
        private Button cancelButton;

        public string UserText { get; private set; } // תכנות לקבלת הטקסט שהקליד המשתמש

        public InputForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // יצירת האלמנטים של תיבת הטקסט
            this.textBox = new TextBox()
            {
                Width = 200,
                Location = new Point(20, 20)
            };

            this.okButton = new Button()
            {
                Text = "אישור",
                Width = 100,
                Location = new Point(20, 50)
            };
            this.okButton.Click += OkButton_Click;

            this.cancelButton = new Button()
            {
                Text = "ביטול",
                Width = 100,
                Location = new Point(130, 50)
            };
            this.cancelButton.Click += CancelButton_Click;

            // הוספת האלמנטים לתיבת הטקסט
            this.Controls.Add(textBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);

            // הגדרת מאפייני הטופס
            this.Text = "מה הכתובת החברה";
            this.Width = 300;
            this.Height = 130;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            UserText = textBox.Text; // שמירת הטקסט שהוזן בתיבת הטקסט
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }




    }



}
