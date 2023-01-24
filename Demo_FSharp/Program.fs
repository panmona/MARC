//Or you can import it straight from a file
open MARC

// let marcRecords = new FileMarc()
// marcRecords.ImportMARC("record2.mrc");

let marcRecords = FileMARC()
marcRecords.ImportMARC("record.mrc")
marcRecords.ImportMARC("record2.mrc")

let mutable i = 0

for record in marcRecords do
    i <- i + 1
    //The Warnings property contains a list of issues that the decoder found with the record.
    //The decoder attempts to return a valid MARC record to the best of it's ability.
    printfn $"Book #%i{i} has been decoded with %i{record.Warnings.Count} errors!"

    //Once decoded you can easily access specific data within the record, as well as make changes.

    //Array notation we will get the first requested tag in the record, or null if one does not exist.
    //First we'll get the Author.  Since there should only be one author tag array notation is the easiest to use.
    let authorField = record.GetField "100"

    //Each tag in the record is a field object. To get the data we have to know if it is a DataField or a ControlField and act accordingly.
    if authorField.IsDataField() then
        let authorDataField = authorField :?> DataField
        let authorName = authorDataField['a']
        printfn $"The author of this book is %s{authorName.Data}"
    elif authorField.IsControlField() then
        printfn "Something went horribly wrong. The author field should never be a Control Field."

    //Now we will get the subjects for this record. Since a book can have multiple subjects we will use GetFields() which returns a List<Field> object.
    //Note: Not passing in a tag number to GetFields will return all the tags in the record.
    let subjects = record.GetFields("650")
    printfn $"Here are the subjects for Book #%i{i}"
    // Here we will assume each Field is actually a DataField since ISBNs should always be a DataField.
    for subject in subjects do
        let subject = subject :?> DataField

        //We also want to loop through each subfield.
        //Just like with GetFields() you can either pass in a subfield value, or nothing to get all the subfields
        subject.GetSubfields()
        |> Seq.map (fun (x: Subfield) -> x.Data)
        |> String.concat " "
        |> printfn "%s"
    
printfn "Press any key to exit."
