
//***TAG BUTTONS ***/
let index = 0;

document.querySelector("form").addEventListener("submit", function () {
    const tagListOptions = document.querySelectorAll("#TagList option");
    // Select all options before form submission & passes tags to the HTTP post
    tagListOptions.forEach(option => option.selected = true); 
});

function AddTag() {
    var tagEntry = document.querySelector("#TagEntry");

    //Create new option for the select list
    //parameters populate both value and the text
    let newOption = new Option(tagEntry.value, tagEntry.value);
    document.querySelector("#TagList").options[index++] = newOption;

    //clear out the tagEntry control
    tagEntry.value = "";
}
function DeleteTag() {
    let tagList = document.querySelector("#TagList");
    let selectedIndex = tagList.selectedIndex;

    if (selectedIndex >= 0) {
        // Removes the selected option
        tagList.remove(selectedIndex); 
        // Decrement index since an option was removed
        index--;  
    }
}

//query tagValues variable for data
if (tagValues != "") {
    let tagArray = tagValues.split(",");
    for (let loop = 0; loop < tagArray.length; loop++) {
        ReplaceTag(tagArray[loop], loop);
        index++;
    }
}

function ReplaceTag(tag, index) {
    let newOption = newOption(tag, tag);
    document.querySelector("#TagList").options[index] = newOption;

}

/**UPDATE FOOT COPYRIGHT YEAR*/
document.addEventListener("DOMContentLoaded", function () {
    const currentYear = new Date().getFullYear();
    document.querySelector("#currentYear").textContent = currentYear;
});
