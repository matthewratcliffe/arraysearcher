using ArraySearch;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("Aysha", "Dr. Ayesha Khan")]
    [TestCase("Aisha", "Dr. Ayesha Khan")]
    [TestCase("Eisha", "Dr. Ayesha Khan")]
    [TestCase("Jon", "Dr. John Smith")]
    [TestCase("Jhon", "Dr. John Smith")]
    [TestCase("Prya", "Dr. Priya Patel")]
    [TestCase("Preya", "Dr. Priya Patel")]
    [TestCase("Jems", "Dr. James Anderson")]
    [TestCase("Jaymes", "Dr. James Anderson")]
    [TestCase("Deepk", "Dr. Deepak Verma")]
    [TestCase("Dipak", "Dr. Deepak Verma")]
    [TestCase("Rebeka", "Dr. Rebecca Santos")]
    [TestCase("Rebeca", "Dr. Rebecca Santos")]
    [TestCase("Ahmad", "Dr. Ahmed Malik")]
    [TestCase("Ahmid", "Dr. Ahmed Malik")]
    [TestCase("Emly", "Dr. Emily Johnson")]
    [TestCase("Emmily", "Dr. Emily Johnson")]
    [TestCase("Arjen", "Dr. Arjun Mehta")]
    [TestCase("Arjoon", "Dr. Arjun Mehta")]
    [TestCase("Sara", "Dr. Sarah Taylor")]
    [TestCase("Saara", "Dr. Sarah Taylor")]
    [TestCase("Migel", "Dr. Miguel Cruz")]
    [TestCase("Mihel", "Dr. Miguel Cruz")]
    [TestCase("Olivea", "Dr. Olivia Brown")]
    [TestCase("Olyvia", "Dr. Olivia Brown")]
    [TestCase("Haroon", "Dr. Haroon Qureshi")]
    [TestCase("Harun", "Dr. Haroon Qureshi")]
    [TestCase("Hana", "Dr. Hannah White")]
    [TestCase("Hanna", "Dr. Hannah White")]
    [TestCase("Rajeef", "Dr. Rajeev Menon")]
    [TestCase("Rajev", "Dr. Rajeev Menon")]
    [TestCase("Enjali", "Dr. Anjali Desai")]
    [TestCase("Anjaly", "Dr. Anjali Desai")]
    [TestCase("Loren", "Dr. Lauren Martin")]
    [TestCase("Lorin", "Dr. Lauren Martin")]
    [TestCase("Rizwan", "Dr. Rizwan Ahmed")]
    [TestCase("Rizuan", "Dr. Rizwan Ahmed")]
    [TestCase("Eathen", "Dr. Ethan Walker")]
    [TestCase("Eithan", "Dr. Ethan Walker")]
    [TestCase("Katreena", "Dr. Katrina Mendoza")]
    [TestCase("Catrina", "Dr. Katrina Mendoza")]
    [TestCase("Naveed", "Dr. Naveed Iqbal")]
    [TestCase("Navid", "Dr. Naveed Iqbal")]
    [TestCase("Kloe", "Dr. Chloe Adams")]
    [TestCase("Chloey", "Dr. Chloe Adams")]
    [TestCase("Binighno", "Dr. Benigno Reyes")]
    [TestCase("Benigno", "Dr. Benigno Reyes")]
    [TestCase("Sofi", "Dr. Sophie Harris")]
    [TestCase("Sofee", "Dr. Sophie Harris")]
    [TestCase("Saira", "Dr. Saira Raza")]
    [TestCase("Sayra", "Dr. Saira Raza")]
    [TestCase("Dilan", "Dr. Dylan Lewis")]
    [TestCase("Dilon", "Dr. Dylan Lewis")]
    public void SoundexTest(string searchText, string expectedValue)
    {
        var searchMethods = new Search();
        var result = searchMethods.DoSearch(TestData.DoctorNames.ToArray(), searchText);
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    // Testing for last names
    [TestCase("Kan", "Dr. Ayesha Khan")]
    [TestCase("Smit", "Dr. John Smith")]
    [TestCase("Patel", "Dr. Priya Patel")]
    [TestCase("Andersen", "Dr. James Anderson")]
    [TestCase("Verma", "Dr. Deepak Verma")]
    [TestCase("Mendosa", "Dr. Katrina Mendoza")]
    public void SoundexLastNameTest(string searchText, string expectedValue)
    {
        var searchMethods = new Search();
        var result = searchMethods.DoSearch(TestData.DoctorNames.ToArray(), searchText);
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    // Testing complex name variations and edge cases
    [TestCase("", "")] // Empty string
    [TestCase("XYZ", "")] // No match
    [TestCase("dr. Ayesha", "Dr. Ayesha Khan")] // With title
    [TestCase("JOHN", "Dr. John Smith")] // All caps
    [TestCase("benjamin", "Dr. Benjamin Lee")] // All lowercase
    [TestCase("Saira-Raza", "Dr. Saira Raza")] // Hyphenated search
    public void SoundexEdgeCaseTest(string searchText, string expectedValue)
    {
        var searchMethods = new Search();
        var result = searchMethods.DoSearch(TestData.DoctorNames.ToArray(), searchText);
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [TestCase("Sara Jonson", "Sarah Johnson")]
    [TestCase("Saara Jonson", "Sarah Johnson")]
    [TestCase("Sarah Jhnson", "Sarah Johnson")]
    [TestCase("Saira", "Saira Khan")]
    [TestCase("Jems", "Jems Nichols")]
    [TestCase("Jaymes", "Jaymes Peterson")]
    [TestCase("Aisha", "Aisha Williams")]
    [TestCase("Eisha", "Eisha Roberts")]
    [TestCase("Miguel", "Miguel Rivera")]
    [TestCase("Mihel", "Mihel Fernandez")]
    [TestCase("Migel", "Migel Castillo")]
    [TestCase("Jon R", "Jon Richardson")]
    [TestCase("John Ham", "John Hamilton")]
    [TestCase("Jhon Ste", "Jhon Stevens")]
    [TestCase("Rebeka", "Rebeka Watson")]
    [TestCase("Rebecca", "Rebecca Morgan")]
    [TestCase("Rebeca", "Rebeca Fisher")]
    [TestCase("Deepk", "Deepk Thomas")]
    [TestCase("Deepak", "Deepak Sharma")]
    [TestCase("Dipak", "Dipak Patel")]
    public void NormalNameSoundexTest(string searchText, string expectedValue)
    {
        var searchMethods = new Search();
        var result = searchMethods.DoSearch(TestData.NormalNames.ToArray(), searchText);
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [TestCase("Kumar", "Rajesh Kumar")]
    [TestCase("Zhang", "Wei Zhang")]
    [TestCase("Tanaka", "Hiroshi Tanaka")]
    [TestCase("Kim", "Jin-ho Kim")]
    [TestCase("Aboud", "Omar Abboud")]
    [TestCase("Diallo", "Amara Diallo")]
    [TestCase("C Hernandez", "Carlos Hernandez")]
    public void NormalNameLastNameTest(string searchText, string expectedValue)
    {
        var searchMethods = new Search();
        var result = searchMethods.DoSearch(TestData.NormalNames.ToArray(), searchText);
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [TestCase("Oluwa", "Oluwaseun Adeyemi")]
    [TestCase("Jin-ho", "Jin-ho Kim")]
    [TestCase("Prakash", "Prakash Iyer")]
    [TestCase("Leila", "Leila Hakim")]
    [TestCase("Isabella", "Isabella Lopez")]
    [TestCase("Arjn M", "Arjun Mehta")]
    [TestCase("Emly", "Emly Bennett")]
    public void NormalNameVariationTest(string searchText, string expectedValue)
    {
        var searchMethods = new Search();
        var result = searchMethods.DoSearch(TestData.NormalNames.ToArray(), searchText);
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void ExactMatch_NormalNames_Test()
    {
        var searchMethods = new Search();
        
        // Western Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Michael Johnson"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Jennifer Smith"), Is.EqualTo("Jennifer Smith"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Christopher Williams"), Is.EqualTo("Christopher Williams"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Elizabeth Brown"), Is.EqualTo("Elizabeth Brown"));
        
        // Eastern Asian Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Wei Zhang"), Is.EqualTo("Wei Zhang"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Hiroshi Tanaka"), Is.EqualTo("Hiroshi Tanaka"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Jin-ho Kim"), Is.EqualTo("Jin-ho Kim"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Yuki Sato"), Is.EqualTo("Yuki Sato"));
        
        // South Asian Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Rajesh Kumar"), Is.EqualTo("Rajesh Kumar"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Priyanka Sharma"), Is.EqualTo("Priyanka Sharma"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Arjun Patel"), Is.EqualTo("Arjun Patel"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Sunita Singh"), Is.EqualTo("Sunita Singh"));
        
        // Middle Eastern Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Ali Al-Mansour"), Is.EqualTo("Ali Al-Mansour"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Leila Hakim"), Is.EqualTo("Leila Hakim"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Ahmed El-Masri"), Is.EqualTo("Ahmed El-Masri"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Yasmin Karimi"), Is.EqualTo("Yasmin Karimi"));
        
        // African Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Kwame Osei"), Is.EqualTo("Kwame Osei"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Amara Diallo"), Is.EqualTo("Amara Diallo"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Oluwaseun Adeyemi"), Is.EqualTo("Oluwaseun Adeyemi"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Fatou Ndiaye"), Is.EqualTo("Fatou Ndiaye"));
        
        // Hispanic/Latino Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Carlos Hernandez"), Is.EqualTo("Carlos Hernandez"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Sofia Rodriguez"), Is.EqualTo("Sofia Rodriguez"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Javier Gonzalez"), Is.EqualTo("Javier Gonzalez"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Isabella Lopez"), Is.EqualTo("Isabella Lopez"));
    }
    
    [Test]
    public void Misspelled_NormalNames_Test()
    {
        var searchMethods = new Search();
        
        // Western Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Mikael Jonson"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Jenifer Smyth"), Is.EqualTo("Jennifer Smith"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Kristofer Willyams"), Is.EqualTo("Christopher Williams"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Elisabet Braun"), Is.EqualTo("Elizabeth Brown"));
        
        // Eastern Asian Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Way Chang"), Is.EqualTo("Wei Zhang"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Hiroshi Tanaca"), Is.EqualTo("Hiroshi Tanaka"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Yuuki Satoh"), Is.EqualTo("Yuki Sato"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Soo Jin Park"), Is.EqualTo("Soo-jin Park"));
        
        // South Asian Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Rajish Kumaar"), Is.EqualTo("Rajesh Kumar"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Preeyanka Sharmah"), Is.EqualTo("Priyanka Sharma"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Arjan Patell"), Is.EqualTo("Arjun Patel"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Suneeta Sing"), Is.EqualTo("Sunita Singh"));
        
        // Middle Eastern Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Alee AlMansour"), Is.EqualTo("Ali Al-Mansour"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Layla Hakeem"), Is.EqualTo("Leila Hakim"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Ahmad ElMasri"), Is.EqualTo("Ahmed El-Masri"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Yasmine Kareemi"), Is.EqualTo("Yasmin Karimi"));
        
        // African Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Quame Osay"), Is.EqualTo("Kwame Osei"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Amarah Dyalo"), Is.EqualTo("Amara Diallo"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Oluaseun Adeyemy"), Is.EqualTo("Oluwaseun Adeyemi"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Fatoo Ndiay"), Is.EqualTo("Fatou Ndiaye"));
        
        // Hispanic/Latino Names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Karlos Ernandes"), Is.EqualTo("Carlos Hernandez"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Sophia Rodriges"), Is.EqualTo("Sofia Rodriguez"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Xavier Gonsales"), Is.EqualTo("Javier Gonzalez"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Izabela Lopes"), Is.EqualTo("Isabella Lopez"));
    }
    
    [Test]
    public void Transliteration_NormalNames_Test()
    {
        var searchMethods = new Search();
        
        // Specific transliteration cases
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Mihel Fernandez"), Is.EqualTo("Mihel Fernandez"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Miguel Fernandez"), Is.EqualTo("Mihel Fernandez"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Miguel Rivera"), Is.EqualTo("Miguel Rivera"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Mihel Rivera"), Is.EqualTo("Miguel Rivera"));
        
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Migel Castillo"), Is.EqualTo("Migel Castillo"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Miguel Castillo"), Is.EqualTo("Migel Castillo"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Mihel Castillo"), Is.EqualTo("Migel Castillo"));
        
        // Other transliteration patterns
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Saira Khan"), Is.EqualTo("Saira Khan"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Sayra Khan"), Is.EqualTo("Saira Khan"));
        
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Deepak Sharma"), Is.EqualTo("Deepak Sharma"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Dipak Sharma"), Is.EqualTo("Deepak Sharma"));
        
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Jon Richardson"), Is.EqualTo("Jon Richardson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "John Richardson"), Is.EqualTo("Jon Richardson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Jhon Richardson"), Is.EqualTo("Jon Richardson"));
        
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Rebecca Morgan"), Is.EqualTo("Rebecca Morgan"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Rebeka Morgan"), Is.EqualTo("Rebecca Morgan"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Rebeca Morgan"), Is.EqualTo("Rebecca Morgan"));
        
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Arjun Mehta"), Is.EqualTo("Arjun Mehta"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Arjen Mehta"), Is.EqualTo("Arjun Mehta"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Arjoon Mehta"), Is.EqualTo("Arjun Mehta"));
    }
    
    [Test]
    public void Initial_And_LastName_NormalNames_Test()
    {
        var searchMethods = new Search();
        
        // Initial + Full Last name
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "M Johnson"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "J Smith"), Is.EqualTo("Jennifer Smith"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "C Williams"), Is.EqualTo("Christopher Williams"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "R Kumar"), Is.EqualTo("Rajesh Kumar"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "A Al-Mansour"), Is.EqualTo("Ali Al-Mansour"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "K Osei"), Is.EqualTo("Kwame Osei"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "C Hernandez"), Is.EqualTo("Carlos Hernandez"));
        
        // Initial with period
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "M. Johnson"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "J. Smith"), Is.EqualTo("Jennifer Smith"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "C. Hernandez"), Is.EqualTo("Carlos Hernandez"));
    }
    
    [Test]
    public void First_And_PartialLast_NormalNames_Test()
    {
        var searchMethods = new Search();
        
        // First + Partial Last name
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Michael John"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Jennifer Sm"), Is.EqualTo("Jennifer Smith"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Christopher Will"), Is.EqualTo("Christopher Williams"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Rajesh Ku"), Is.EqualTo("Rajesh Kumar"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Ali Al"), Is.EqualTo("Ali Al-Mansour"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Kwame Os"), Is.EqualTo("Kwame Osei"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Carlos Hern"), Is.EqualTo("Carlos Hernandez"));
    }
    
    [Test]
    public void FirstName_Only_NormalNames_Test()
    {
        var searchMethods = new Search();
        
        // First name only
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Michael"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Jennifer"), Is.EqualTo("Jennifer Smith"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Christopher"), Is.EqualTo("Christopher Williams"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Rajesh"), Is.EqualTo("Rajesh Kumar"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Ali"), Is.EqualTo("Ali Al-Mansour"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Kwame"), Is.EqualTo("Kwame Osei"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Carlos"), Is.EqualTo("Carlos Hernandez"));
    }
    
    [Test]
    public void LastName_Only_NormalNames_Test()
    {
        var searchMethods = new Search();
        
        // Last name only
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Johnson"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Smith"), Is.EqualTo("Jennifer Smith"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Williams"), Is.EqualTo("Christopher Williams"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Kumar"), Is.EqualTo("Rajesh Kumar"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Al-Mansour"), Is.EqualTo("Ali Al-Mansour"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Osei"), Is.EqualTo("Kwame Osei"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Hernandez"), Is.EqualTo("Carlos Hernandez"));
    }
    
    [Test]
    public void EdgeCases_NormalNames_Test()
    {
        var searchMethods = new Search();
        
        // All caps
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "MICHAEL JOHNSON"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "JENNIFER SMITH"), Is.EqualTo("Jennifer Smith"));
        
        // All lowercase
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "michael johnson"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "jennifer smith"), Is.EqualTo("Jennifer Smith"));
        
        // Hyphenated search for non-hyphenated names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Michael-Johnson"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Rajesh-Kumar"), Is.EqualTo("Rajesh Kumar"));
        
        // Non-hyphenated search for hyphenated names
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Jin ho Kim"), Is.EqualTo("Jin-ho Kim"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Ali Al Mansour"), Is.EqualTo("Ali Al-Mansour"));
        
        // With titles/prefixes
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Mr. Michael Johnson"), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "Ms. Jennifer Smith"), Is.EqualTo("Jennifer Smith"));
        
        // Trailing/leading spaces
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), " Michael Johnson "), Is.EqualTo("Michael Johnson"));
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), " Jennifer Smith "), Is.EqualTo("Jennifer Smith"));
        
        // Empty string
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), ""), Is.EqualTo(""));
        
        // No match
        Assert.That(searchMethods.DoSearch(TestData.NormalNames.ToArray(), "XYZ"), Is.EqualTo(""));
    }
    
    [Test]
    public void MihelvsMiguel_Specific_Test()
    {
        var searchMethods = new Search();
        
        // This specifically tests the failing case in the original issue
        var result = searchMethods.DoSearch(TestData.DoctorNames.ToArray(), "Mihel");
        Assert.That(result, Is.EqualTo("Dr. Miguel Cruz"));
        
        // Test the reverse as well
        var result2 = searchMethods.DoSearch(TestData.DoctorNames.ToArray(), "Miguel");
        Assert.That(result2, Is.EqualTo("Dr. Miguel Cruz"));
        
        // Make sure we don't get "Dr. George Mitchell" for either search
        Assert.That(result, Is.Not.EqualTo("Dr. George Mitchell"));
        Assert.That(result2, Is.Not.EqualTo("Dr. George Mitchell"));
    }
}
