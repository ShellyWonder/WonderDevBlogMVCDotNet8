
//***TAG BUTTONS ***/
let index = 0;

document.querySelector("form").addEventListener("submit", function () {
    const tagListOptions = document.querySelectorAll("#TagList option");
    // Select all options before form submission & passes tags to the HTTP post
    tagListOptions.forEach(option => option.selected = true); 
});

function AddTag() {
    var tagEntry = document.querySelector("#TagEntry");
    //mechanism to detect an error state
    let searchResult = Search(tagEntry.value);

    if (searchResult != null) {
        //trigger alert modal
        swalWithDarkButton.fire({
            html: `<span class="font-weight-bolder">${searchResult.toUpperCase()}</span>`
        });
    }
    else {
    //Create new option for the select list
    //parameters populate both value and the text
    let newOption = new Option(tagEntry.value, tagEntry.value);
    document.querySelector("#TagList").options[index++] = newOption;
    }

    //clear out the tagEntry control
    tagEntry.value = "";
    return true;
}
function DeleteTag() {
    let tagCount = 1;
    let tagList = document.querySelector("#TagList");
    if (!tagList) return false;
    let selectedIndex = tagList.selectedIndex;

    if (selectedIndex >= 0) {
        // Removes the selected option
        tagList.remove(selectedIndex); 
        // Decrement index since an option was removed
        index--;  
    }
    if (tagList.selectedIndex == -1) {
        swalWithDarkButton.fire({
            html: "<span class='font-weight-bolder'>CHOOSE A TAG BEFORE DELETING</span>"
        });
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
    let newOption = new Option(tag, tag);
    document.querySelector("#TagList").options[index] = newOption;

}

/**UPDATE FOOT COPYRIGHT YEAR*/
document.addEventListener("DOMContentLoaded", function () {
    const currentYear = new Date().getFullYear();
    document.querySelector("#currentYear").textContent = currentYear;
});

//Search() detects an empty and/or duplicate tag & returns an error string
function Search(str) {
    if (str === "") {
        return "Empty tags are not permitted.";
    }
    varEl = document.querySelector("#TagList");
    if (tagsEl) {
        let options = tagsEl.options;
        for (let index = 0; index < options.length; index++) {
            if (options[index].value == str) {
                return "The Tag #${str} is a duplicate and not permitted.";
            }
        }
    }
}

const swalWithDarkButton = Swal.mixin({
    customClass:{
        confirmButton: 'btn btn-danger btn-sm btn-block btn-outline-dark'
    },
    imageUrl: '/img/default_icon.png',
    timer: 3000,
    buttonStyling: false
})
