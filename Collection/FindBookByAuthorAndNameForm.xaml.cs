using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic; 
using System.IO;                  
using System.Linq;               
using System.Text;                 
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Collection
{
    public partial class FindBookByAuthorAndNameForm : Window
    {
        public FindBookByAuthorAndNameForm()
        {
            InitializeComponent();
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string authorInput = AuthorTextBox.Text.Trim();
            string nameInput = NameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(authorInput) || string.IsNullOrEmpty(nameInput))
            {
                MessageBox.Show("Будь ласка, введіть автора та назву книги.", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataAccess dataAccess = new DataAccess();
            Book foundBook = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(dataAccess.connectionString))
                {
                    conn.Open();
                    string query = "SELECT id, author, name, year, rack_number, shell_number FROM collection WHERE author = @author AND name = @name";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@author", authorInput);
                        cmd.Parameters.AddWithValue("@name", nameInput);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                foundBook = new Book(
                                    Convert.ToInt32(reader["id"]),
                                    reader["name"].ToString(),
                                    reader["author"].ToString(),
                                    Convert.ToInt32(reader["shell_number"]),
                                    Convert.ToInt32(reader["rack_number"]),
                                    Convert.ToInt32(reader["year"])
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час пошуку книги: {ex.Message}", "Помилка бази даних", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            if (foundBook != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                string safeAuthor = foundBook.Author.Replace(" ", "_");
                string safeName = foundBook.Name.Replace(" ", "_");
                saveFileDialog.FileName = $"Книга_{safeAuthor}_{safeName}";
                saveFileDialog.DefaultExt = ".docx";
                saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";

                bool? result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    string fileName = saveFileDialog.FileName;
                    try
                    {
                        using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
                        {
                            MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                            mainPart.Document = new Document();
                            Body body = mainPart.Document.AppendChild(new Body());

                            Paragraph titlePara = body.AppendChild(new Paragraph());
                            Run titleRun = titlePara.AppendChild(new Run());
                            titleRun.AppendChild(new Text($"Інформація про книгу: \"{foundBook.Name}\" автора {foundBook.Author}"));
                            titleRun.RunProperties = new RunProperties(new Bold(), new FontSize() { Val = "28" });
                            titlePara.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });

                            body.AppendChild(new Paragraph());

                            Table table = new Table();
                            TableProperties tblProps = new TableProperties(new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }, new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }, new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }, new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }));
                            table.AppendChild(tblProps);

                            TableRow trHeader = new TableRow();
                            string[] headers = { "ID", "Назва", "Автор", "Рік", "Стелаж", "Полиця" };
                            foreach (string headerText in headers)
                            {
                                TableCell tc = new TableCell(new Paragraph(new Run(new Text(headerText)) { RunProperties = new RunProperties(new Bold()) }));
                                tc.Append(new TableCellProperties(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }));
                                trHeader.Append(tc);
                            }
                            table.Append(trHeader);

                            TableRow trData = new TableRow();
                            trData.Append(new TableCell(new Paragraph(new Run(new Text(foundBook.Id.ToString())))));
                            trData.Append(new TableCell(new Paragraph(new Run(new Text(foundBook.Name)))));
                            trData.Append(new TableCell(new Paragraph(new Run(new Text(foundBook.Author)))));
                            trData.Append(new TableCell(new Paragraph(new Run(new Text(foundBook.Year.ToString())))));
                            trData.Append(new TableCell(new Paragraph(new Run(new Text(foundBook.Rack_Number.ToString())))));
                            trData.Append(new TableCell(new Paragraph(new Run(new Text(foundBook.Shell_Number.ToString())))));
                            table.Append(trData);

                            body.Append(table);
                        }
                        MessageBox.Show($"Файл успішно збережено за шляхом: {fileName}", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при записі до файлу: {ex.Message}", "Помилка запису", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Дану книгу не знайдено в базі даних.", "Результат пошуку", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();
        }
    }
}