using System.Net.Http;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Runtime.CompilerServices;

namespace Translator_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {// This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
        static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

        private static string translateKey = Environment.GetEnvironmentVariable("TRANSLATE_KEY");
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";

        // location, also known as region.
        // required if you're using a multi-service or regional (not global) resource. It can be found in the Azure portal on the Keys and Endpoint page.
        private static string location = Environment.GetEnvironmentVariable("LOCATION");

        static string[,] languages = new string[,]
        { {"Африкаанс", "af"}, {"Албанский", "sq"}, {"Амхарский", "am"},
        {"Арабский", "ar"}, {"Армянский", "hy"}, {"Ассамский", "as"},
        {"Азербайджанская (латиница)", "az"}, {"Бенгальский", "bn"},
        {"Башкирский", "ba"}, {"Баскский (Баскский)", "eu"}, {"Bhojpuri", "bho"},
        {"Бодо", "brx"}, {"Боснийский (латиница)", "bs"}, {"Болгарский", "bg"},
        {"Кантонский (традиционное письмо)", "yue"}, {"Каталанский", "ca"},
        {"Китайский (литературный)", "lzh"}, {"китайский (упрощенный)", "zh-Hans"},
        {"китайский (традиционный)", "zh-Hant"}, {"chiShona", "sn"}, {"Хорватский", "hr"},
        {"чешский", "cs"}, {"датский", "da"}, {"Дари", "prs"}, {"Дивихай", "dv"}, {"Догри", "doi"},
        {"Голландский", "nl"}, {"Английский", "en"}, {"Эстонский", "et"}, {"Фарерский", "fo"}, {"Фиджи", "fj"},
        {"Филиппинский", "fil"}, {"Финский", "fi"}, {"французский", "fr"}, {"Французский (Канада)", "fr-ca"},
        {"Галисийский", "gl"}, {"Грузинский", "ka"}, {"немецкий", "de"}, {"Греческий", "el"}, {"Гуджарати", "gu"},
        {"Гаитянский креольский", "ht"}, {"Хауса", "ha"}, {"Иврит", "he"}, {"Хинди", "hi"}, {"Хмонг дау (латиница)", "mww"},
        {"Венгерский", "hu"}, {"Исландский", "is"}, {"Игбо", "ig"}, {"Индонезийский", "id"}, {"Инуиннактун", "ikt"},
        {"Инуктитут", "iu"}, {"Inuktitut (латиница)", "iu-Latn"}, {"Ирландский", "ga"}, {"Итальянский", "it"},
        {"Японский", "ja"}, {"Каннада", "kn"}, {"Кашмира", "ks"}, {"Казахский", "kk"}, {"Кхмерский", "km"},
        {"Киньяруанда", "rw"}, {"Клингонский", "tlh-Latn"}, {"Клингонский (пикад)", "tlh-Piqd"},
        {"Конкани", "gom"}, {"Корейский", "ko"}, {"Курдский (Сорани)", "ku"}, {"Курдский (Северный)", "kmr"},
        {"Киргизский (кириллица)", "ky"}, {"Lao", "lo"}, {"Латышский", "lv"}, {"Литовский", "lt"},
        {"Лингала", "ln"}, {"Нижнелужицкий", "dsb"}, {"Луганда", "lug"}, {"Macedonian", "mk"},
        {"Майтили", "mai"}, {"Малагасийский", "mg"}, {"Малайский (латиница)", "ms"}, {"Малаялам", "ml"},
        {"Мальтийский", "mt"}, {"Маори", "mi"}, {"Маратхи", "mr"}, {"Монгольский (кириллица)", "mn-Cyrl"},
        {"Монгольский (старомонгольское письмо)", "mn-Mong"}, {"Мьянма", "my"}, {"Непальский", "ne"},
        {"Норвежский", "nb"}, {"Ньянджа", "nya"}, {"Ория", "or"}, {"Пушту", "ps"}, {"Персидский", "fa"},
        {"Польский", "pl"}, {"португальский (Бразилия)", "pt"}, {"Португальский (Португалия)", "pt-pt"},
        {"Панджаби", "pa"}, {"Отоми — Керетаро", "otq"}, {"Румынский", "ro"}, {"Rundi", "run"},
        {"русский", "ru"}, {"Самоанский (латиница)", "sm"}, {"Сербский (кириллица)", "sr-Cyrl"},
        {"Сербский (латиница)", "sr-Latn"}, {"Южный сото", "st"}, {"Северный сото", "nso"},
        {"Тсвана", "tn"}, {"Синдхи", "sd"}, {"Сингальский", "si"}, {"Словацкий", "sk"}, {"Словенский", "sl"},
        {"Сомали (арабское письмо)", "so"}, {"Испанский", "es"}, {"Суахили (латиница)", "sw"},
        {"Шведский", "sv"}, {"Таитянский", "ty"}, {"Тамильский", "ta"}, {"Татарский (латиница)", "tt"},
        {"Телугу", "te"}, {"Тайский", "th"}, {"Тибетский", "bo"}, {"Тигринья", "ti"}, {"Тонганский", "to"},
        {"Турецкий", "tr"}, {"Туркменский (латиница)", "tk"}, {"Украинский", "uk"}, {"Верхнелужицкий", "hsb"},
        {"Урду", "ur"}, {"Уйгурский (арабское письмо)", "ug"}, {"Узбекский (латиница)", "uz"},
        {"Вьетнамский", "vi"}, {"Валлийский", "cy"}, {"Коса", "xh"}, {"Йоруба", "yo"},
        {"Юкатекский майя", "yua"}, {"Зулусский", "zu"} };


        public MainWindow()
        {
            InitializeComponent();
            AddLangFrom(this.ComboBoxFrom);
            AddLangFrom(this.ComboBoxTo);
            btnTranslate.IsEnabled = false;
            btnClear.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.texboxlang1.Clear();
            this.texboxlang2.Clear();
        }

        private void AddLangFrom(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.ItemsSource = GetLangs();
            comboBox.DisplayMemberPath = "Lang_Name";
            comboBox.SelectedValuePath = "Lang";
            comboBox.SelectedIndex = 0;
        }

        private List<LangItem> GetLangs()
        {
            List<LangItem> items = new List<LangItem>();

            for (int i = 0; i < languages.GetLength(0); i++)
                items.Add(new LangItem(languages[i, 1], languages[i, 0]));

            return items;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)//перевести
        {
            string langCodeFrom = GetLangCode(this.ComboBoxFrom);
            string langCodeTo = GetLangCode(this.ComboBoxTo);
            String str = await Translate(this.texboxlang1.Text, langCodeFrom, langCodeTo);

            var obj = JsonConvert.DeserializeObject<Translator_WPF.Responce[]>(str);
            this.texboxlang2.Text = obj[0].translations[0].text;
        }

        private string GetLangCode(ComboBox comboBox)
        {
            LangItem selectedItem = comboBox.SelectedItem as LangItem;
            return selectedItem.Lang;
        }



        async static Task<string> Translate(string textToTranslate, string from, string to)
        {
            // Input and output languages are defined as parameters.
            string route = "/translate?api-version=3.0&from=" + from + "&to=" + to;


            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;// Build the request.
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", translateKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);// location required if you're using a multi-service or regional (not global) resource.

                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);// Send the request and get response. 
                string result = await response.Content.ReadAsStringAsync();// Read response as a string. 
                return result;
            }
        }

        private void texboxlang1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                btnTranslate.IsEnabled = true;
                btnClear.IsEnabled = true;
            }
            else
            {
                btnTranslate.IsEnabled = false;
                btnClear.IsEnabled = false;
            }
        }

    }
}