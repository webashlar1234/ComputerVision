$(document).ready(function () {
     $(".spin-loader").hide();
    var filesInput = document.getElementById("uploadImage");
    filesInput.addEventListener("change", function (event) {
        $("#uploadImage").attr("disabled", "disabled");
        var files = event.target.files;
        var formData = new FormData();        
        if (files.length <= 0) {
            wanotification.showWarningNofificationMessage("Please select an image", { closeButton: true});
            $("#uploadImage").removeAttr("disabled");
            return false;
        }
        var file = files[0];
        if (!file.type.match('image')) {
            wanotification.showWarningNofificationMessage('Supported image formats: JPEG, PNG, GIF, BMP.', { closeButton: true });
            $("#uploadImage").removeAttr("disabled");
            return false;
        }
        if ((file.size / (1024 * 1024)) > 4) { 
            wanotification.showWarningNofificationMessage('Image file size must be less than 4MB.', { closeButton: true });
            $("#uploadImage").removeAttr("disabled");
            return false;
        }
        var output = document.getElementById("result");
        $(output).empty();
        $("#divTags").empty();
        var strLoaderText = '<span class="spin-loader fnt-size"><i class="fa fa-spinner spin ml-1 text-white"></i>';
        strLoaderText += '<span class="text-white"> Loading. . .</span ></span>';
        $("#imgId").html(strLoaderText);
        $(".spin-loader").show();
        formData.append("files", file);
        var picReader = new FileReader();
        picReader.addEventListener("load", function (event) {
            var picFile = event.target;
            var div = document.createElement("div");
            $(div).css("position", "relative");
            $(div).attr("id", "divPreviewImg");
            $(div).attr("class", "text-center");
            div.innerHTML = "<img class='thumbnail img-fluid' id='previewImg' alt='Responsive image' src='" + picFile.result + "' />";
            output.insertBefore(div, null);
        });
        picReader.readAsDataURL(file);
        //Ajax call to server;
        $.ajax({
            type: "POST",
            url: '/Home/Upload',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                try {
                    if (response) {

                        $("#imgId").html("Image ID : <span>" + response.imageId + "</span>");

                        if (response.description != null) {
                            if (response.description.tags != null) {
                                if (response.description.tags.length > 0) {
                                    var strTags = "";
                                    jQuery.each(response.description.tags, function (i, val) {
                                        strTags += '<a href="#">' + val + '</a>';
                                    });
                                    $("#divTags").html(strTags);
                                }
                                else {
                                    $("#divTags").html('<p>No tags found for this image.</p>');
                                }
                            }
                            else {
                                $("#divTags").html('<p>No tags found for this image.</p>');
                            }
                        }

                        var $expandEle = $(".expand").find(">:first-child");
                        if ($expandEle.text() == "+") {
                            $(".expand").click();
                        }
                        if (response.objects != null) {

                            var mainWidth = response.metadata.width;
                            var mainHeight = response.metadata.height;

                            var prevImg = $("#previewImg");
                            var newImgWidth = prevImg.width();
                            var newImgHeight = prevImg.height();

                            var widthRatio = newImgWidth * 100 / mainWidth;
                            var heightRatio = newImgHeight * 100 / mainHeight;

                            jQuery.each(response.objects, function (i, val) {
                                var rect = val.rectangle;
                                var divParent = $("#divPreviewImg");
                                var div = document.createElement("div");
                                div.setAttribute('data-toggle', 'tooltip');
                                div.setAttribute('title', val.object);
                                div.style.position = "absolute";
                                div.style.height = ((heightRatio * rect.h) / 100) + "px";
                                div.style.width = ((widthRatio * rect.w) / 100) + "px";
                                div.style.border = "solid 3px #ba0b93";
                                div.style.top = ((heightRatio * rect.y) / 100) + "px"; // Y val
                                div.style.left = ((widthRatio * rect.x) / 100) + "px"; // X val
                                $(divParent).append(div);
                            });
                        }
                    }
                } catch (e) { wanotification.showErrorNofificationMessage("Unable to process response.", { closeButton: true }); }
                $(".spin-loader").hide();
                $("#uploadImage").removeAttr("disabled");
            },
            error: function (error) {
                wanotification.showErrorNofificationMessage("Something went wrong please try again!!!", { closeButton: true });
                $(".spin-loader").hide();
                $("#uploadImage").removeAttr("disabled");
            }
        });
    });
});