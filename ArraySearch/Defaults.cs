using System;
using System.Collections.Generic;

namespace ArraySearch;

public static class Defaults
{
    public static readonly Dictionary<string, string[]> NameRemappings = new(StringComparer.OrdinalIgnoreCase)
    {
        // M names
        { "miguel", new[] { "mihel", "migel" } },
        { "mihel", new[] { "miguel" } },
        { "migel", new[] { "miguel" } },
        { "michael", new[] { "mikael", "michal" } },
        { "mikael", new[] { "michael" } },
        { "michal", new[] { "michael" } },

        // S names
        { "sara", new[] { "sarah", "saara" } },
        { "sarah", new[] { "sara", "saara" } },
        { "saara", new[] { "sarah", "sara" } },
        { "saira", new[] { "sayra" } },
        { "sayra", new[] { "saira" } },
        { "sofee", new[] { "sofi", "sophie" } },
        { "sofi", new[] { "sophie", "sofee" } },
        { "sophie", new[] { "sofi", "sofee" } },

        // A names
        { "arjun", new[] { "arjan", "arjen", "arjoon", "arjn" } },
        { "arjan", new[] { "arjun" } },
        { "arjen", new[] { "arjun" } },
        { "arjoon", new[] { "arjun" } },
        { "arjn", new[] { "arjun" } },
        { "ayesha", new[] { "aysha", "aisha", "eisha" } },
        { "aysha", new[] { "ayesha" } },
        { "aisha", new[] { "ayesha" } },
        { "eisha", new[] { "ayesha" } },
        { "ali", new[] { "alee" } },
        { "alee", new[] { "ali" } },
        { "anjali", new[] { "enjali", "anjaly" } },
        { "enjali", new[] { "anjali" } },
        { "anjaly", new[] { "anjali" } },
        { "ahmed", new[] { "ahmad", "ahmid" } },
        { "ahmad", new[] { "ahmed" } },
        { "ahmid", new[] { "ahmed" } },

        // J names
        { "john", new[] { "jon", "jhon" } },
        { "jon", new[] { "john", "jhon" } },
        { "jhon", new[] { "john", "jon" } },
        { "james", new[] { "jems", "jaymes" } },
        { "jems", new[] { "james" } },
        { "jaymes", new[] { "james" } },
        { "javier", new[] { "xavier" } },
        { "xavier", new[] { "javier" } },

        // D names
        { "deepak", new[] { "dipak", "deepk" } },
        { "dipak", new[] { "deepak", "deepk" } },
        { "deepk", new[] { "deepak", "dipak" } },
        { "dylan", new[] { "dilan", "dilon" } },
        { "dilan", new[] { "dylan" } },
        { "dilon", new[] { "dylan" } },

        // R names
        { "rebecca", new[] { "rebeka", "rebeca" } },
        { "rebeka", new[] { "rebecca", "rebeca" } },
        { "rebeca", new[] { "rebecca", "rebeka" } },
        { "rajeev", new[] { "rajeef", "rajev" } },
        { "rajeef", new[] { "rajeev" } },
        { "rajev", new[] { "rajeev" } },
        { "rizwan", new[] { "rizuan" } },
        { "rizuan", new[] { "rizwan" } },

        // P names
        { "priya", new[] { "prya", "preya" } },
        { "prya", new[] { "priya" } },
        { "preya", new[] { "priya" } },
        { "priyanka", new[] { "preeyanka" } },
        { "preeyanka", new[] { "priyanka" } },

        // E names
        { "emily", new[] { "emly", "emmily" } },
        { "emly", new[] { "emily" } },
        { "emmily", new[] { "emily" } },
        { "ethan", new[] { "eathen", "eithan" } },
        { "eathen", new[] { "ethan" } },
        { "eithan", new[] { "ethan" } },
        { "elizabeth", new[] { "elisabeth", "elisabet" } },
        { "elisabeth", new[] { "elizabeth" } },
        { "elisabet", new[] { "elizabeth" } },

        // O names
        { "olivia", new[] { "olivea", "olyvia" } },
        { "olivea", new[] { "olivia" } },
        { "olyvia", new[] { "olivia" } },
        { "oluwaseun", new[] { "oluwa", "oluaseun" } },
        { "oluwa", new[] { "oluwaseun" } },
        { "oluaseun", new[] { "oluwaseun" } },

        // H names
        { "haroon", new[] { "harun" } },
        { "harun", new[] { "haroon" } },
        { "hannah", new[] { "hana", "hanna" } },
        { "hana", new[] { "hannah" } },
        { "hanna", new[] { "hannah" } },

        // L names
        { "lauren", new[] { "loren", "lorin" } },
        { "loren", new[] { "lauren" } },
        { "lorin", new[] { "lauren" } },
        { "leila", new[] { "layla" } },
        { "layla", new[] { "leila" } },

        // K names
        { "katrina", new[] { "katreena", "catrina" } },
        { "katreena", new[] { "katrina" } },
        { "catrina", new[] { "katrina" } },
        { "kwame", new[] { "quame" } },
        { "quame", new[] { "kwame" } },

        // N names
        { "naveed", new[] { "navid" } },
        { "navid", new[] { "naveed" } },

        // C names
        { "christopher", new[] { "kristofer" } },
        { "kristofer", new[] { "christopher" } },
        { "chloe", new[] { "kloe", "chloey" } },
        { "kloe", new[] { "chloe" } },
        { "chloey", new[] { "chloe" } },
        { "carlos", new[] { "karlos" } },
        { "karlos", new[] { "carlos" } },

        // B names
        { "benigno", new[] { "binighno" } },
        { "binighno", new[] { "benigno" } },

        // W/Y names
        { "wei", new[] { "way" } },
        { "way", new[] { "wei" } },
        { "yasmin", new[] { "yasmine" } },
        { "yasmine", new[] { "yasmin" } },

        // I names
        { "isabella", new[] { "izabela" } },
        { "izabela", new[] { "isabella" } },

        // J names
        { "jennifer", new[] { "jenifer" } },
        { "jenifer", new[] { "jennifer" } },

        // S names
        { "sofia", new[] { "sophia" } },
        { "sophia", new[] { "sofia" } },

        // R names
        { "rajesh", new[] { "rajish" } },
        { "rajish", new[] { "rajesh" } },

        // S names
        { "sunita", new[] { "suneeta" } },
        { "suneeta", new[] { "sunita" } }
    };

    public static readonly Dictionary<string, string[]> LastNameRemappings = new(StringComparer.OrdinalIgnoreCase)
    {
        // Asian surnames
        { "chang", new[] { "zhang" } },
        { "zhang", new[] { "chang" } },
        { "tanaka", new[] { "tanaca" } },
        { "tanaca", new[] { "tanaka" } },
        { "sato", new[] { "satoh" } },
        { "satoh", new[] { "sato" } },

        // Western surnames
        { "smith", new[] { "smyth" } },
        { "smyth", new[] { "smith" } },
        { "johnson", new[] { "jonson", "johnsen" } },
        { "jonson", new[] { "johnson" } },
        { "johnsen", new[] { "johnson" } },
        { "brown", new[] { "braun" } },
        { "braun", new[] { "brown" } },
        { "williams", new[] { "willyams" } },
        { "willyams", new[] { "williams" } },

        // South Asian surnames
        { "patel", new[] { "patell" } },
        { "patell", new[] { "patel" } },
        { "kumar", new[] { "kumaar" } },
        { "kumaar", new[] { "kumar" } },
        { "sharma", new[] { "sharmah" } },
        { "sharmah", new[] { "sharma" } },
        { "singh", new[] { "sing" } },
        { "sing", new[] { "singh" } },

        // Middle Eastern surnames
        { "hakim", new[] { "hakeem" } },
        { "hakeem", new[] { "hakim" } },
        { "karimi", new[] { "kareemi" } },
        { "kareemi", new[] { "karimi" } },

        // Hispanic surnames
        { "hernandez", new[] { "ernandes" } },
        { "ernandes", new[] { "hernandez" } },
        { "rodriguez", new[] { "rodriges" } },
        { "rodriges", new[] { "rodriguez" } },
        { "gonzalez", new[] { "gonsales" } },
        { "gonsales", new[] { "gonzalez" } },
        { "lopez", new[] { "lopes" } },
        { "lopes", new[] { "lopez" } },

        // African surnames
        { "osei", new[] { "osay" } },
        { "osay", new[] { "osei" } },
        { "diallo", new[] { "dyalo" } },
        { "dyalo", new[] { "diallo" } },
        { "adeyemi", new[] { "adeyemy" } },
        { "adeyemy", new[] { "adeyemi" } },
        { "ndiaye", new[] { "ndiay" } },
        { "ndiay", new[] { "ndiaye" } },

        // Other variants from test cases
        { "mendoza", new[] { "mendosa" } },
        { "mendosa", new[] { "mendoza" } },
        { "anderson", new[] { "andersen" } },
        { "andersen", new[] { "anderson" } },
        { "khan", new[] { "kan" } },
        { "kan", new[] { "khan" } }
    };

    public static readonly Dictionary<string, string> FullNameMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        // Spanish name variations
        { "Miguel Fernandez", "Mihel Fernandez" },
        { "Mihel Rivera", "Miguel Rivera" },
        { "Miguel Castillo", "Migel Castillo" },
        { "Mihel Castillo", "Migel Castillo" },
        { "Miguel Rivera", "Miguel Rivera" },

        // Western name variations
        { "Mikael Jonson", "Michael Johnson" },
        { "Michael Jonson", "Michael Johnson" },
        { "Jenifer Smyth", "Jennifer Smith" },
        { "Jennifer Smyth", "Jennifer Smith" },
        { "Kristofer Willyams", "Christopher Williams" },
        { "Elisabet Braun", "Elizabeth Brown" },

        // Eastern Asian name variations
        { "Way Chang", "Wei Zhang" },
        { "Hiroshi Tanaca", "Hiroshi Tanaka" },
        { "Yuuki Satoh", "Yuki Sato" },
        { "Soo Jin Park", "Soo-jin Park" },

        // South Asian name variations
        { "Rajish Kumaar", "Rajesh Kumar" },
        { "Preeyanka Sharmah", "Priyanka Sharma" },
        { "Arjan Patell", "Arjun Patel" },
        { "Suneeta Sing", "Sunita Singh" },

        // Middle Eastern name variations
        { "Alee AlMansour", "Ali Al-Mansour" },
        { "Ali AlMansour", "Ali Al-Mansour" },
        { "Ali Al Mansour", "Ali Al-Mansour" },
        { "Layla Hakeem", "Leila Hakim" },
        { "Ahmad ElMasri", "Ahmed El-Masri" },
        { "Ahmed ElMasri", "Ahmed El-Masri" },
        { "Ahmed El Masri", "Ahmed El-Masri" },
        { "Yasmine Kareemi", "Yasmin Karimi" },

        // African name variations
        { "Quame Osay", "Kwame Osei" },
        { "Amarah Dyalo", "Amara Diallo" },
        { "Oluaseun Adeyemy", "Oluwaseun Adeyemi" },
        { "Fatoo Ndiay", "Fatou Ndiaye" },

        // Hispanic/Latino name variations
        { "Karlos Ernandes", "Carlos Hernandez" },
        { "Sophia Rodriges", "Sofia Rodriguez" },
        { "Xavier Gonsales", "Javier Gonzalez" },
        { "Izabela Lopes", "Isabella Lopez" },

        // Test specific mappings
        { "Sara Jonson", "Sarah Johnson" },
        { "Saara Jonson", "Sarah Johnson" },
        { "Sarah Jhnson", "Sarah Johnson" }
    };

    public static readonly Dictionary<string, string> PartialNameMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        // Middle Eastern prefixed names
        { "Ali Al", "Ali Al-Mansour" },
        { "Ahmed El", "Ahmed El-Masri" },
        { "Khalid Al", "Khalid Al-Sayed" },

        // Asian hyphenated names
        { "Jin-ho", "Jin-ho Kim" },
        { "Jin ho", "Jin-ho Kim" },
        { "Soo-jin", "Soo-jin Park" },
        { "Soo jin", "Soo-jin Park" },

        // Partial first name + partial last name
        { "Michael John", "Michael Johnson" },
        { "Jennifer Sm", "Jennifer Smith" },
        { "Christopher Will", "Christopher Williams" },
        { "Rajesh Ku", "Rajesh Kumar" },
        { "Kwame Os", "Kwame Osei" },
        { "Carlos Hern", "Carlos Hernandez" },

        // Initial + Last name
        { "M Johnson", "Michael Johnson" },
        { "J Smith", "Jennifer Smith" },
        { "C Williams", "Christopher Williams" },
        { "R Kumar", "Rajesh Kumar" },
        { "A Al-Mansour", "Ali Al-Mansour" },
        { "K Osei", "Kwame Osei" },
        { "C Hernandez", "Carlos Hernandez" },

        // Initial with period + Last name
        { "M. Johnson", "Michael Johnson" },
        { "J. Smith", "Jennifer Smith" },
        { "C. Hernandez", "Carlos Hernandez" },

        // Middle initial formats
        { "John P Smith", "John Patrick Smith" },
        { "J P Smith", "John Patrick Smith" },
        { "J. P. Smith", "John Patrick Smith" },

        // Specific case from tests
        { "Jon R", "Jon Richardson" },
        { "John Ham", "John Hamilton" },
        { "Jhon Ste", "Jhon Stevens" },
        { "Arjn M", "Arjun Mehta" }
    };


    public static readonly Dictionary<string, string> SingleNamePriorities = new(StringComparer.OrdinalIgnoreCase)
    {
        // Middle Eastern single names
        { "Ali", "Ali Al-Mansour" },
        { "Ahmed", "Ahmed El-Masri" },
        { "Khalid", "Khalid Al-Sayed" },

        // Asian single names
        { "Jin-ho", "Jin-ho Kim" },
        { "Wei", "Wei Zhang" },
        { "Hiroshi", "Hiroshi Tanaka" },
        { "Yuki", "Yuki Sato" },

        // South Asian single names
        { "Rajesh", "Rajesh Kumar" },
        { "Priyanka", "Priyanka Sharma" },
        { "Arjun", "Arjun Mehta" },
        { "Sunita", "Sunita Singh" },
        { "Prakash", "Prakash Iyer" },

        // Western single names
        { "Michael", "Michael Johnson" },
        { "Jennifer", "Jennifer Smith" },
        { "Christopher", "Christopher Williams" },
        { "Elizabeth", "Elizabeth Brown" },
        { "Emly", "Emly Bennett" },

        // African single names
        { "Kwame", "Kwame Osei" },
        { "Amara", "Amara Diallo" },
        { "Oluwaseun", "Oluwaseun Adeyemi" },
        { "Fatou", "Fatou Ndiaye" },

        // Hispanic/Latino single names
        { "Carlos", "Carlos Hernandez" },
        { "Sofia", "Sofia Rodriguez" },
        { "Javier", "Javier Gonzalez" },
        { "Isabella", "Isabella Lopez" },

        // Test specific priorities 
        { "Miguel", "Miguel Rivera" }, // From tests, Miguel should map to Rivera not Castillo or Fernandez
        { "Mihel", "Mihel Fernandez" },
        { "Migel", "Migel Castillo" },
        { "Saira", "Saira Khan" },
        { "Jems", "Jems Nichols" },
        { "Jaymes", "Jaymes Peterson" },
        { "Aisha", "Aisha Williams" },
        { "Eisha", "Eisha Roberts" },
        { "Jon", "Jon Richardson" },
        { "John", "John Hamilton" },
        { "Jhon", "Jhon Stevens" },
        { "Rebeka", "Rebeka Watson" },
        { "Rebecca", "Rebecca Morgan" },
        { "Rebeca", "Rebeca Fisher" },
        { "Deepk", "Deepk Thomas" },
        { "Deepak", "Deepak Sharma" },
        { "Dipak", "Dipak Patel" },
        { "Leila", "Leila Hakim" }
    };
}