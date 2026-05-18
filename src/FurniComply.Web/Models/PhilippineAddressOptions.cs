using System;
using System.Collections.Generic;
using System.Linq;

namespace FurniComply.Web.Models;

public static class PhilippineAddressOptions
{
    private static readonly IReadOnlyList<string> ProvinceOptions =
        new[]
        {
            "Abra",
            "Agusan del Norte",
            "Agusan del Sur",
            "Aklan",
            "Albay",
            "Antique",
            "Apayao",
            "Aurora",
            "Basilan",
            "Bataan",
            "Batanes",
            "Batangas",
            "Benguet",
            "Biliran",
            "Bohol",
            "Bukidnon",
            "Bulacan",
            "Cagayan",
            "Camarines Norte",
            "Camarines Sur",
            "Camiguin",
            "Capiz",
            "Catanduanes",
            "Cavite",
            "Cebu",
            "Cotabato",
            "Davao de Oro",
            "Davao del Norte",
            "Davao del Sur",
            "Davao Occidental",
            "Davao Oriental",
            "Dinagat Islands",
            "Eastern Samar",
            "Guimaras",
            "Ifugao",
            "Ilocos Norte",
            "Ilocos Sur",
            "Iloilo",
            "Isabela",
            "Kalinga",
            "La Union",
            "Laguna",
            "Lanao del Norte",
            "Lanao del Sur",
            "Leyte",
            "Maguindanao del Norte",
            "Maguindanao del Sur",
            "Marinduque",
            "Masbate",
            "Metro Manila",
            "Misamis Occidental",
            "Misamis Oriental",
            "Mountain Province",
            "Negros Occidental",
            "Negros Oriental",
            "Northern Samar",
            "Nueva Ecija",
            "Nueva Vizcaya",
            "Occidental Mindoro",
            "Oriental Mindoro",
            "Palawan",
            "Pampanga",
            "Pangasinan",
            "Quezon",
            "Quirino",
            "Rizal",
            "Romblon",
            "Samar",
            "Sarangani",
            "Siquijor",
            "Sorsogon",
            "South Cotabato",
            "Southern Leyte",
            "Sultan Kudarat",
            "Sulu",
            "Surigao del Norte",
            "Surigao del Sur",
            "Tarlac",
            "Tawi-Tawi",
            "Zambales",
            "Zamboanga del Norte",
            "Zamboanga del Sur",
            "Zamboanga Sibugay"
        };

    private static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> BarangaysByProvinceInternal =
        new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Abra"] = new[] { "Bangbangar", "Calaba", "Dolores", "Poblacion", "Zone 1", "Zone 2" },
            ["Agusan del Norte"] = new[] { "Aupagan", "Bonbon", "Doongan", "Kinamlutan", "Poblacion", "Taligaman" },
            ["Agusan del Sur"] = new[] { "Aguinaldo", "Bunawan Brook", "Consuelo", "Poblacion", "San Teodoro", "Tagbobonga" },
            ["Aklan"] = new[] { "Andagao", "Balabag", "Banga", "Estancia", "Poblacion", "Tigayon" },
            ["Albay"] = new[] { "Bagumbayan", "Bigaa", "Cabangan", "Dinagaan", "Poblacion", "Rawis" },
            ["Antique"] = new[] { "Aningalan", "Badiangan", "Mag-aba", "Molo", "Poblacion", "San Pedro" },
            ["Apayao"] = new[] { "Calafug", "Dibagat", "Kabugao", "Luna", "Poblacion", "San Isidro" },
            ["Aurora"] = new[] { "Baler", "Buhangin", "Calabuanan", "Debucao", "Poblacion", "Sabang" },
            ["Basilan"] = new[] { "Balatanay", "Bato-bato", "Bohe Yakan", "Campo Uno", "Latuan", "Tabuk" },
            ["Bataan"] = new[] { "Bagumbayan", "Cupang Proper", "Ibayo", "Poblacion", "Talisay", "Tuyo" },
            ["Batanes"] = new[] { "Chanarian", "Raele", "Salagao", "San Antonio", "San Vicente", "Torongan" },
            ["Batangas"] = new[] { "Balagtas", "Banaba Center", "Bolbok", "Concepcion", "Poblacion", "San Jose Sico" },
            ["Benguet"] = new[] { "Ambiong", "Balili", "Betag", "La Trinidad", "Pico", "Poblacion" },
            ["Biliran"] = new[] { "Atipolo", "Bato", "Caraycaray", "Naval", "Poblacion", "Sanggalang" },
            ["Bohol"] = new[] { "Bool", "Cabawan", "Cogon", "Dao", "Poblacion", "Taloto" },
            ["Metro Manila"] = new[]
            {
                "Barangay 183",
                "Barangay 649",
                "Bagong Pag-asa",
                "Batasan Hills",
                "Commonwealth",
                "Fort Bonifacio"
            },
            ["Bukidnon"] = new[] { "Alae", "Bagontaas", "Casisang", "Kisanday", "Poblacion", "Sumpong" },
            ["Bulacan"] = new[]
            {
                "Bagna",
                "Balite",
                "Cutcot",
                "Kaypian",
                "Muzon",
                "Poblacion"
            },
            ["Cagayan"] = new[] { "Bagay", "Balzain", "Cataggaman", "Carig", "Poblacion", "Tanza" },
            ["Camarines Norte"] = new[] { "Anameam", "Bagasbas", "Binanuahan", "Calasgasan", "Poblacion", "Talobatib" },
            ["Camarines Sur"] = new[] { "Concepcion Grande", "Del Rosario", "Liboton", "Pacol", "Panicuason", "Triangulo" },
            ["Camiguin"] = new[] { "Agoho", "Benoni", "Catarman", "Mambajao", "Poblacion", "Yumbing" },
            ["Capiz"] = new[] { "Baybay", "Bolo", "Lanot", "Poblacion", "Tiza", "Tanque" },
            ["Catanduanes"] = new[] { "Balongbong", "Binanwahan", "Calatagan", "San Roque", "Sipi", "Tobrehon" },
            ["Cavite"] = new[]
            {
                "Alapan I-A",
                "Anabu I-C",
                "Bucandala",
                "Manggahan",
                "Palico",
                "San Agustin"
            },
            ["Laguna"] = new[]
            {
                "Aplaya",
                "Balibago",
                "Canlubang",
                "Dita",
                "Paciano Rizal",
                "Poblacion"
            },
            ["Cebu"] = new[]
            {
                "Apas",
                "Banilad",
                "Basak San Nicolas",
                "Guadalupe",
                "Lahug",
                "Mabolo"
            },
            ["Cotabato"] = new[] { "Amas", "Aroman", "Awang", "Kalaisan", "Poblacion", "Rosary Heights" },
            ["Davao de Oro"] = new[] { "Andili", "Cabidianan", "Mangayon", "Naboc", "Poblacion", "San Miguel" },
            ["Davao del Norte"] = new[] { "Apokon", "Carmen", "Mankilam", "Magugpo Poblacion", "San Miguel", "Visayan Village" },
            ["Davao del Sur"] = new[]
            {
                "Buhangin",
                "Catalunan Grande",
                "Matina",
                "Mintal",
                "Sasa",
                "Talomo"
            },
            ["Davao Occidental"] = new[] { "Anibongan", "Buhangin", "Little Baguio", "Poblacion", "San Pedro", "Tubalan" },
            ["Davao Oriental"] = new[] { "Dahican", "Mati", "Sainz", "Central", "Don Enrique Lopez", "Matiao" },
            ["Dinagat Islands"] = new[] { "Cab-ilan", "Don Ruben", "Mabini", "Poblacion", "San Jose", "Villa Ecleo" },
            ["Eastern Samar"] = new[] { "Bonghon", "Can-avid", "Guindapunan", "Poblacion", "Roxas", "Tinago" },
            ["Guimaras"] = new[] { "Alegre", "Canhawan", "Concordia", "Poblacion", "San Miguel", "Tando" },
            ["Ifugao"] = new[] { "Baguinge", "Bokiawan", "Lagawe", "Poblacion East", "Poblacion West", "Pugol" },
            ["Ilocos Norte"] = new[] { "Barangay 1", "Barangay 2", "Barangay 39", "Barangay 55-B", "Nangalisan", "Suyo" },
            ["Ilocos Sur"] = new[] { "Ayusan Norte", "Ayusan Sur", "Bongtolan", "Cabaroan", "Poblacion", "Tamag" },
            ["Iloilo"] = new[] { "Balabago", "Calajunan", "Jaro", "La Paz", "Mandurriao", "Molo" },
            ["Isabela"] = new[] { "Alibagu", "Bagumbayan", "District 1", "District 2", "Poblacion", "San Fermin" },
            ["Kalinga"] = new[] { "Appas", "Bulanao", "Dagupan Centro", "Laya East", "Poblacion", "Tuga" },
            ["La Union"] = new[] { "Biday", "Catbangen", "Masicong", "Poblacion", "San Fernando", "Tanqui" },
            ["Lanao del Norte"] = new[] { "Bagong Silang", "Dalipuga", "Pala-o", "Poblacion", "Saray", "Tipanoy" },
            ["Lanao del Sur"] = new[] { "Bangon", "Basak Malutlut", "Matampay", "Pantar", "Poblacion", "Sagonsongan" },
            ["Leyte"] = new[] { "Anibong", "Baras", "Downtown", "Poblacion", "San Jose", "Tacloban" },
            ["Maguindanao del Norte"] = new[] { "Awang", "Bakat", "Dalican", "Poblacion", "Rosary Heights", "Semba" },
            ["Maguindanao del Sur"] = new[] { "Buluan", "Datu Paglas", "Kulasi", "Poblacion", "Reina Regente", "Tumbao" },
            ["Marinduque"] = new[] { "Bagtingon", "Bangbang", "Ipil", "Malusak", "Poblacion", "Tanza" },
            ["Masbate"] = new[] { "Asid", "Bagumbayan", "Centro", "Espinosa", "Poblacion", "Tugbo" },
            ["Misamis Occidental"] = new[] { "Aguada", "Bagakay", "Burgos", "Carmen Annex", "Poblacion", "Tinago" },
            ["Misamis Oriental"] = new[] { "Agusan", "Balulang", "Carmen", "Gusa", "Nazareth", "Poblacion" },
            ["Mountain Province"] = new[] { "Balili", "Caluttit", "Poblacion", "Samoki", "Sinto", "Tadian" },
            ["Negros Occidental"] = new[] { "Alijis", "Banago", "Mansilingan", "Poblacion", "Tangub", "Villamonte" },
            ["Negros Oriental"] = new[] { "Bagacay", "Batinguel", "Calindagan", "Candau-ay", "Poblacion", "Tabuc-tubig" },
            ["Northern Samar"] = new[] { "Balite", "Cabay", "Dalakit", "Ipil-ipil", "Poblacion", "Urdaneta" },
            ["Nueva Ecija"] = new[] { "Abar 1st", "Bantug", "Caalibangbangan", "Mabini", "Poblacion", "San Josef Norte" },
            ["Nueva Vizcaya"] = new[] { "Bagahabag", "District IV", "Don Mariano Perez", "Magsaysay", "Poblacion", "Roxas" },
            ["Occidental Mindoro"] = new[] { "Bayanan", "Burgos", "Lumangbayan", "Poblacion", "San Jose", "Sibalat" },
            ["Oriental Mindoro"] = new[] { "Bagong Bayan", "Bucayao", "Camilmil", "Poblacion", "Sta. Isabel", "Suqui" },
            ["Palawan"] = new[] { "Bancao-bancao", "Irawan", "Mandaragat", "San Jose", "Sicsican", "Tiniguiban" },
            ["Pampanga"] = new[] { "Balibago", "Cutcut", "Del Carmen", "Poblacion", "San Jose", "Santo Rosario" },
            ["Pangasinan"] = new[] { "Bonuan", "Lucao", "Malued", "Poblacion", "Tapuac", "Tambac" },
            ["Quezon"] = new[] { "Bocohan", "Gulang-gulang", "Ibabang Dupay", "Isabang", "Poblacion", "Talipan" },
            ["Quirino"] = new[] { "Cabarroguis", "Dibibi", "Gundaway", "Poblacion", "Tucod", "Villa Ventura" },
            ["Rizal"] = new[] { "Bagumbayan", "Beverly Hills", "Manggahan", "Mayamot", "San Jose", "Santa Cruz" },
            ["Romblon"] = new[] { "Agbaluto", "Bagacay", "Lonos", "Poblacion", "Sablayan", "Tambac" },
            ["Samar"] = new[] { "Acedillo", "Balud", "Canlapwas", "Poblacion", "San Jose", "Sinangtan" },
            ["Sarangani"] = new[] { "Baliton", "Calumpang", "Luma", "Poblacion", "Tinoto", "Upper Klinan" },
            ["Siquijor"] = new[] { "Candaping B", "Cang-agong", "Lazi", "Poblacion", "Tongo", "Tubod" },
            ["Sorsogon"] = new[] { "Balogo", "Bibincahan", "Cabid-an", "Poblacion", "Sampaloc", "Sirangan" },
            ["South Cotabato"] = new[] { "Calumpang", "General Paulino Santos", "Koronadal Proper", "Lagao", "Poblacion", "Tambler" },
            ["Southern Leyte"] = new[] { "Asuncion", "Combado", "Ichon", "Poblacion", "Suba", "Tunga-tunga" },
            ["Sultan Kudarat"] = new[] { "Bagumbayan", "Kalawag II", "Kenram", "Poblacion", "Sampao", "Tacurong" },
            ["Sulu"] = new[] { "Asturias", "Bus-bus", "Kajatian", "Poblacion", "Takut-takut", "Tulayan" },
            ["Surigao del Norte"] = new[] { "Bagakay", "Luna", "Poblacion", "Sabang", "Taft", "Washington" },
            ["Surigao del Sur"] = new[] { "Bagyang", "Canlanipa", "Mabua", "Poblacion", "San Juan", "Washington" },
            ["Tarlac"] = new[] { "Balanti", "Cut-cut I", "Maliwalo", "Poblacion", "San Vicente", "Tibag" },
            ["Tawi-Tawi"] = new[] { "Bato-bato", "Bongao Poblacion", "Lato-lato", "Malassa", "Pahut", "Tandubas" },
            ["Zambales"] = new[] { "Asinan", "Barretto", "East Bajac-bajac", "Poblacion", "Sta. Rita", "West Tapinac" },
            ["Zamboanga del Norte"] = new[] { "Biasong", "Dicayas", "Minaog", "Poblacion", "Sicayab", "Turno" },
            ["Zamboanga del Sur"] = new[] { "Arena Blanco", "Bolong", "Divisoria", "Pasonanca", "Putik", "Tetuan" },
            ["Zamboanga Sibugay"] = new[] { "Bagong Oroquieta", "Camanga", "Dumalinao", "Poblacion", "Sanito", "Tigpalay" }
        };

    public static IReadOnlyList<string> Provinces => ProvinceOptions;

    public static IReadOnlyDictionary<string, IReadOnlyList<string>> BarangaysByProvince => BarangaysByProvinceInternal;

    public static string ComposeAddress(string? streetAddress, string? barangay, string? province)
    {
        var parts = new[] { streetAddress?.Trim(), barangay?.Trim(), province?.Trim() }
            .Where(x => !string.IsNullOrWhiteSpace(x));
        return string.Join(", ", parts);
    }

    public static (string StreetAddress, string Barangay, string Province) ParseAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return (string.Empty, string.Empty, string.Empty);

        var parts = address
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        if (parts.Length >= 3)
        {
            var province = parts[^1];
            var barangay = parts[^2];
            var streetAddress = string.Join(", ", parts.Take(parts.Length - 2));
            return (streetAddress, barangay, province);
        }

        if (parts.Length == 2)
            return (parts[0], string.Empty, parts[1]);

        return (parts[0], string.Empty, string.Empty);
    }
}
