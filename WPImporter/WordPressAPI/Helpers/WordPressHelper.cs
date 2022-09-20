using System.Globalization;
using WPImporter.Database.Models;
using WPImporter.GoogleAPI.Models;
using WPImporter.WordPressAPI.Extensions;
using WPImporter.WordPressAPI.Models;

namespace WPImporter.WordPressAPI.Helpers
{
    public static class WordPressHelper
    {
        public static int GetRegionId(int voivodeshipId)
        {
            // FIRST INT - RegionId from WordPress
            // SECOND INT - VoivodeshipId from DataBase

            var regionIds = new Dictionary<int, int>
            {
                { 1, 0 }, // ERROR województwa
                { 2, 36 },
                { 3, 32 },
                { 4, 73 },
                { 5, 70 },
                { 6, 71 },
                { 7, 74 },
                { 8, 75 },
                { 9, 76 },
                { 10, 77 },
                { 11, 78 },
                { 12, 79 },
                { 13, 80 },
                { 14, 81 },
                { 15, 82 },
                { 16, 83 },
                { 17, 84 },
                { 18, 0 } // BRAK znalezionego województwa
            };

            return regionIds[voivodeshipId];
        }

        public static ListingMetaData CreateListingMetaData(Company company, PlaceDetails? placeDetails, string? placeId)
        {
            var metaDatas = new List<MetaData>();

            // 1. Dane z bazy danych
            var startDate = company.StartDate.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);

            metaDatas.AddMetaData(new MetaData(company.Nip, "_nip"));
            metaDatas.AddMetaData(new MetaData(company.Krs, "_krs"));
            metaDatas.AddMetaData(new MetaData(company.City, "_miasto"));
            metaDatas.AddMetaData(new MetaData(company.Street, "_ulica"));
            metaDatas.AddMetaData(new MetaData(company.BuildingNumber, "_numer_budynku"));
            metaDatas.AddMetaData(new MetaData(company.FlatNumber, "_numer_mieszkania"));
            metaDatas.AddMetaData(new MetaData(company.Voivodeship.Name, "_wojewodztwo"));
            metaDatas.AddMetaData(new MetaData(company.MainPkd, "_gowny_pkd"));
            metaDatas.AddMetaData(new MetaData(company.LegalForm.Shortcut, "_forma_prawna"));
            metaDatas.AddMetaData(new MetaData(company.ClassificationSchema.DisplayName, "_brana"));
            metaDatas.AddMetaData(new MetaData(startDate, "_start_dzialalnosci"));
            metaDatas.AddMetaData(new MetaData(company.PostalCode, "_kod_pocztowy"));
            metaDatas.AddMetaData(new MetaData($"Sprawdzona firma z branży fotowoltaicznej {company.Name.ToLower()}", "keywords"));

            if (company.Email != null)
            {
                metaDatas.AddMetaData(new MetaData(company.Email.ToLower(), "_email"));
            }
            else
            {
                metaDatas.AddMetaData(new MetaData("firma@sprawdzonafotowoltaika.pl", "_email"));
            }

            if (placeDetails != null && placeDetails.result.formatted_address != null)
            {
                metaDatas.AddMetaData(new MetaData(placeDetails.result.formatted_phone_number, "_phone"));
            }
            else
            {
                metaDatas.AddMetaData(new MetaData(company.PhoneNumber, "_phone"));
            }

            if (placeDetails != null && placeDetails.result.website != null)
            {
                metaDatas.AddMetaData(new MetaData(placeDetails.result.website, "_website"));
            }
            else
            {
                metaDatas.AddMetaData(new MetaData(company.WebsiteUrl, "_website"));
            }

            // 2. Dane z Google API
            metaDatas.AddMetaData(new MetaData(placeId, "_place_id"));

            if (placeDetails != null)
            {
                metaDatas.AddMetaData(new MetaData(placeDetails.result.formatted_address, "_address"));
                metaDatas.AddMetaData(new MetaData(placeDetails.result.vicinity, "_friendly_address"));
                metaDatas.AddMetaData(new MetaData(placeDetails.result.geometry.location.lng.ToString(), "_geolocation_long"));
                metaDatas.AddMetaData(new MetaData(placeDetails.result.geometry.location.lat.ToString(), "_geolocation_lat"));

                var openingHours = placeDetails.result.opening_hours;
                if (openingHours != null)
                {
                    foreach (var period in openingHours.periods)
                    {
                        var results = ConvertPeriodToWordPressFormat(period);

                        metaDatas.AddMetaDatas(results);
                    }
                }
            }

            if (placeDetails == null || placeDetails.result.opening_hours == null)
            {
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:4:\"9:00\";}", "_monday_opening_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:5:\"17:00\";}", "_monday_closing_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:4:\"9:00\";}", "_tuesday_opening_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:5:\"17:00\";}", "_tuesday_closing_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:4:\"9:00\";}", "_wednesday_opening_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:5:\"17:00\";}", "_wednesday_closing_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:4:\"9:00\";}", "_thursday_opening_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:5:\"17:00\";}", "_thursday_closing_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:4:\"9:00\";}", "_friday_opening_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:5:\"17:00\";}", "_friday_closing_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:4:\"9:00\";}", "_saturday_opening_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:5:\"17:00\";}", "_saturday_closing_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:4:\"9:00\";}", "_sunday_opening_hour"));
                metaDatas.AddMetaData(new MetaData("a:1:{i:0;s:5:\"17:00\";}", "_sunday_closing_hour"));
            }

            // 3. Dane konfiguracyjne pod Listeo PRO
            metaDatas.AddMetaData(new MetaData("on", "_opening_hours_status"));
            metaDatas.AddMetaData(new MetaData("0", "_verified"));
            metaDatas.AddMetaData(new MetaData("0", "_store_section_status"));
            metaDatas.AddMetaData(new MetaData("0", "_store_widget_status"));
            metaDatas.AddMetaData(new MetaData("0", "_slots_status"));
            metaDatas.AddMetaData(new MetaData("0", "_coupon_for_widget"));
            metaDatas.AddMetaData(new MetaData("0", "_hide_pricing_if_bookable"));
            metaDatas.AddMetaData(new MetaData("on", "_email_contact_widget"));
            metaDatas.AddMetaData(new MetaData("0", "_menu_status"));
            metaDatas.AddMetaData(new MetaData("0", "_booking_status"));
            metaDatas.AddMetaData(new MetaData("0", "_instant_booking"));
            metaDatas.AddMetaData(new MetaData("", "_featured"));
            metaDatas.AddMetaData(new MetaData("pay_now", "_payment_option"));
            metaDatas.AddMetaData(new MetaData("0", "_count_per_guest"));
            metaDatas.AddMetaData(new MetaData("sent", "new_listing_email_notification"));
            metaDatas.AddMetaData(new MetaData("top", "_gallery_style"));
            metaDatas.AddMetaData(new MetaData("new", "_classifieds_condition"));
            metaDatas.AddMetaData(new MetaData("Europe/Warsaw", "_listing_timezone"));

            // 4. Do obsługi na na kiedyś
            // new MetaData("https://www.youtube.com/watch?v=iFGkCpETyCA", "_video"),
            // new MetaData("https://TUTAJ-MOZNA-DAC-STRONE-FIRMY.me", "_booking_link"),

            var listingMetaDate = new ListingMetaData
            {
                MetaDatas = metaDatas
            };

            return listingMetaDate;
        }

        public static string CreateCompanyDescription(Company company)
        {
            return $"Opis przygotowany pod SEO z dynamicznami wstawkami jak ta " +
                $"nazwa firmy np: {company.Name}, " +
                $"o formie prawnej {company.LegalForm.Name}, " +
                $"z branży {company.ClassificationSchema.Name.ToLower()}";
        }

        private static List<MetaData> ConvertPeriodToWordPressFormat(Period period)
        {
            var mapingData = new Dictionary<int, string>
            {
                { 1, "monday" },
                { 2, "tuesday" },
                { 3, "wednesday" },
                { 4, "thursday" },
                { 5, "friday" },
                { 6, "saturday" },
                { 7, "sunday" }
            };

            var day = mapingData[period.open.day];

            var openKey = $"_{day}_opening_hour";
            var closeKey = $"_{day}_closing_hour";

            var openValue = "a:1:{i:0;s:4:\"" + ConvertHour(period.open.time) + "\";}";
            var closeValue = "a:1:{i:0;s:5:\"" + ConvertHour(period.close.time) + "\";}";

            var result = new List<MetaData>
            {
                new MetaData(openValue, openKey),
                new MetaData(closeValue, closeKey)
            };

            return result;
        }

        private static string ConvertHour(string hour)
        {
            var hourPart = "";
            if (hour[0] == '0')
            {
                hourPart = hour[1].ToString();
            }
            else
            {
                hourPart = hour[0..2];
            }

            var minutePart = hour[2..4];

            var result = $"{hourPart}:{minutePart}";

            return result;
        }
    }
}
