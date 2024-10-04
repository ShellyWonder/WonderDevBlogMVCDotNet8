let index = 0;

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
    let tagCount = 1;
    while (tagCount > 0) {
        let tagList = document.querySelector("#TagList");
        let selectedIndex = tagList.selectedIndex;

        if (selectedIndex >= 0) {
            document.querySelector("#TagList").options[selectedIndex] = null;
            --tagCount;
        }
        else {
            tagCount = 0;
            index--;
        }
    }

}

//JQuery:"on submit" highlights all the created tags accompanying the post; passes the tags along the HTTP Post
$("form").on("submit", function () {
    $("#TagList option").prop("selected", "selected");
})