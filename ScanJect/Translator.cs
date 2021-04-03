using System;

using Google.Apis.Translate.v2;
using Google.Cloud.Translation.V2;

using Google.Apis.Services;

namespace ScanJect.Translator
{
   public partial class TranslateSample
    {
        static public string TranslateText2(string foreign_lang, string text)
        {
            string apiKeyTranslate = "AIzaSyB3E-FnWzOuBvGMDNOgW-Te2wyM4tM5Cg0";
            var service = new TranslateService(new BaseClientService.Initializer { ApiKey = apiKeyTranslate });
            var client = new TranslationClientImpl(service, TranslationModel.ServiceDefault);

            Console.OutputEncoding = System.Text.Encoding.UTF8;
			TranslationResult response = client.TranslateText(
                text: text,
                sourceLanguage: "en",
                targetLanguage: foreign_lang);

            return response.TranslatedText;
        }

        static public String langSwitch(String item)
        {
			switch (item)
			{
				case "Afrikaans":
					return "af";
				case "Albanian":
					return "sq";
				case "Amharic":
					return "am";
				case "Arabic":
					return "ar";
				case "Armenian":
					return "hy";
				case "Azerbaijani":
					return "az";
				case "Basque":
					return "eu";
				case "Belarusian":
					return "be";
				case "Bengali":
					return "bn";
				case "Bosnian":
					return "bs";
				case "Bulgarian":
					return "bg";
				case "Catalan":
					return "ca";
				case "Cebuano":
					return "ceb";
				case "Chinese (Simplified)":
					return "zh-CN";
				case "Chinese (Traditional)":
					return "zh-TW";
				case "Corsican":
					return "co";
				case "Croatian":
					return "hr";
				case "Czech":
					return "cs";
				case "Danish":
					return "da";
				case "Dutch":
					return "nl";
				case "English":
					return "en";
				case "Esperanto":
					return "eo";
				case "Estonian":
					return "et";
				case "Finnish":
					return "fi";
				case "French":
					return "fr";
				case "Frisian":
					return "fy";
				case "Galician":
					return "gl";
				case "Georgian":
					return "ka";
				case "German":
					return "de";
				case "Greek":
					return "el";
				case "Gujarati":
					return "gu";
				case "Haitian Creole":
					return "ht";
				case "Hausa":
					return "ha";
				case "Hawaiian":
					return "haw";
				case "Hebrew":
					return "he";
				case "Hindi":
					return "hi";
				case "Hmong":
					return "hmn";
				case "Hungarian":
					return "hu";
				case "Icelandic":
					return "is";
				case "Igbo":
					return "ig";
				case "Indonesian":
					return "id";
				case "Irish":
					return "ga";
				case "Italian":
					return "it";
				case "Japanese":
					return "ja";
				case "Javanese":
					return "jv";
				case "Kannada":
					return "kn";
				case "Kazakh":
					return "kk";
				case "Khmer":
					return "km";
				case "Kinyarwanda":
					return "rw";
				case "Korean":
					return "ko";
				case "Kurdish":
					return "ku";
				case "Kyrgyz":
					return "ky";
				case "Lao":
					return "lo";
				case "Latin":
					return "la";
				case "Latvian":
					return "lv";
				case "Lithuanian":
					return "lt";
				case "Luxembourgish":
					return "lb";
				case "Macedonian":
					return "mk";
				case "Malagasy":
					return "mg";
				case "Malay":
					return "ms";
				case "Malayalam":
					return "ml";
				case "Maltese":
					return "mt";
				case "Maori":
					return "mi";
				case "Marathi":
					return "mr";
				case "Mongolian":
					return "mn";
				case "Myanmar (Burmese)":
					return "my";
				case "Nepali":
					return "ne";
				case "Norwegian":
					return "no";
				case "Nyanja (Chichewa)":
					return "ny";
				case "Odia (Oriya)":
					return "or";
				case "Pashto":
					return "ps";
				case "Persian":
					return "fa";
				case "Polish":
					return "pl";
				case "Portuguese (Portugal, Brazil)":
					return "pt";
				case "Punjabi":
					return "pa";
				case "Romanian":
					return "ro";
				case "Russian":
					return "ru";
				case "Samoan":
					return "sm";
				case "Scots Gaelic":
					return "gd";
				case "Serbian":
					return "sr";
				case "Sesotho":
					return "st";
				case "Shona":
					return "sn";
				case "Sindhi":
					return "sd";
				case "Sinhala Sinhalese":
					return "si";
				case "Slovak":
					return "sk";
				case "Slovenian":
					return "sl";
				case "Somali":
					return "so";
				case "Spanish":
					return "es";
				case "Sundanese":
					return "su";
				case "Swahili":
					return "sw";
				case "Swedish":
					return "sv";
				case "Tagalog (Filipino)":
					return "tl";
				case "Tajik":
					return "tg";
				case "Tamil":
					return "ta";
				case "Tatar":
					return "tt";
				case "Telugu":
					return "te";
				case "Thai":
					return "th";
				case "Turkish":
					return "tr";
				case "Turkmen":
					return "tk";
				case "Ukrainian":
					return "uk";
				case "Urdu":
					return "ur";
				case "Uyghur":
					return "ug";
				case "Uzbek":
					return "uz";
				case "Vietnamese":
					return "vi";
				case "Welsh":
					return "cy";
				case "Xhosa":
					return "xh";
				case "Yiddish":
					return "yi";
				case "Yoruba":
					return "yo";
				case "Zulu":
					return "zu";
				default:
					return "en";
			}
		}
	}
}