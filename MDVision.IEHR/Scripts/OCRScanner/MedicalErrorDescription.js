
//Error Handler for Medical Card Scan Lib
function MedErrors(value)
{
        switch(value)
		{
            case -1: //if medical card's file not found
                alert("Medical Lib : File not Found"); //prompt error message
                break;
            case -2: //if couldn't load file
                alert("Medical Lib : nnot Load File"); //prompt error message
                break;
            case -3: //if no data extracted
                alert("Medical Lib : No Lines Found"); //prompt error message
                break;
		    case -4: //if image not loaded
                alert("Medical Lib : Image not Loaded"); //prompt error message
                break;
            case -5: //if field invalid
                alert("Medical Lib : Invalid Field"); //prompt error message
                break;
            case -6: //if no data found
                alert("Medical Lib : No Data"); //prompt error message
                break;
            case -7: //if last field of data being extracted
                alert("Medical Lib : Last Data"); //prompt error message
                break;
            case -8: //if more fields to follow exist
                alert("Medical Lib : Next Data Exists"); //prompt error message
                break;
			default:
				alert("Medical Lib : Error - " + value);
		}
}
