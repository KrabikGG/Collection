using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Collection
{
    public partial class FindAllBooksByYearForm : Window
    {
        private DataAccess _dataAccess;
        public List<Book> TestHook_BooksFoundForReport { get; private set; }

        public System.Windows.Controls.ListBox GetYearsListBoxForTest() => YearsListBox;

        public FindAllBooksByYearForm(DataAccess dataAccessToUse)
        {
            InitializeComponent();
            this._dataAccess = dataAccessToUse;
        }

        public FindAllBooksByYearForm() : this(new DataAccess())
        {
        }

        public void FindAllBooksByYearForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoadYears();
        }

        public void LoadYears()
        {
            try
            {
                if (_dataAccess == null || _dataAccess.bookList == null || !_dataAccess.bookList.Any())
                {
                    MessageBox.Show("Список книг порожній або не завантажений.", "Помилка даних", MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (FindButton != null) FindButton.IsEnabled = false;
                    return;
                }
                var years = _dataAccess.bookList.Select(book => book.Year).Distinct().OrderBy(year => year).ToList();
                if (YearsListBox != null)
                {
                    if (years.Any())
                    {
                        YearsListBox.ItemsSource = years;
                    }
                    else
                    {
                        MessageBox.Show("У базі даних немає книг для відображення років.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (FindButton != null) FindButton.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження років: {ex.Message}", "Критична помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                if (FindButton != null) FindButton.IsEnabled = false;
            }
        }

        public void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (YearsListBox.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, виберіть рік зі списку.", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_dataAccess == null || _dataAccess.bookList == null)
            {
                MessageBox.Show("Список книг не завантажений.", "Помилка даних", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int selectedYear = (int)YearsListBox.SelectedItem;
            List<Book> booksFound = _dataAccess.bookList.Where(book => book.Year == selectedYear).ToList();

            TestHook_BooksFoundForReport = booksFound;

            if (booksFound.Any())
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = $"Книги_за_{selectedYear}_рік";
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
                            titleRun.AppendChild(new Text($"Книги, видані у {selectedYear} році"));
                            titleRun.RunProperties = new RunProperties(new Bold(), new FontSize() { Val = "32" });
                            titlePara.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });

                            body.AppendChild(new Paragraph());

                            Table table = new Table();
                            TableProperties tblProps = new TableProperties(new TableBorders(
                                new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }, new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }, new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }, new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }));
                            table.AppendChild(tblProps);

                            TableRow trHeader = new TableRow();
                            string[] headers = { "ID", "Назва", "Автор", "Стелаж", "Полиця" };
                            foreach (string header in headers)
                            {
                                TableCell tc = new TableCell(new Paragraph(new Run(new Text(header)) { RunProperties = new RunProperties(new Bold()) }));
                                trHeader.Append(tc);
                            }
                            table.Append(trHeader);

                            foreach (var book in booksFound)
                            {
                                TableRow tr = new TableRow();
                                tr.Append(new TableCell(new Paragraph(new Run(new Text(book.Id.ToString())))));
                                tr.Append(new TableCell(new Paragraph(new Run(new Text(book.Name)))));
                                tr.Append(new TableCell(new Paragraph(new Run(new Text(book.Author)))));
                                tr.Append(new TableCell(new Paragraph(new Run(new Text(book.Rack_Number.ToString())))));
                                tr.Append(new TableCell(new Paragraph(new Run(new Text(book.Shell_Number.ToString())))));
                                table.Append(tr);
                            }
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
                MessageBox.Show("Книг з таким роком не знайдено.", "Результат пошуку", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}