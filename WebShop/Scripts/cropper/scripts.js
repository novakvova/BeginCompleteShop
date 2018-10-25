$(function () {
    var $canvas = $("#canvas"),
        context = $canvas.get(0).getContext('2d');
    $("#dragModeMove").on("click", function () {
        $('#canvas').cropper('setDragMode', 'move');
    });

    $("#dragModeCrop").on("click", function () {
        $('#canvas').cropper('setDragMode', 'crop');
    });
    var x = 1;
    $("#scaleX").on("click", function () {
        $('#canvas').cropper('scaleX', x *= -1);

    });
    var y = 1;
    $("#scaleY").on("click", function () {
        $('#canvas').cropper('scaleY', y *= -1);
    });

    $("#rotateLeft").on("click", function () {
        $('#canvas').cropper('rotate', 45);
    });

    $("#rotateRight").on("click", function () {
        $('#canvas').cropper('rotate', -45);
    });

    $("#moveLeft").on("click", function () {
        $('#canvas').cropper('move', -50, 0);
    });

    $("#moveRight").on("click", function () {
        $('#canvas').cropper('move', 50, 0);
    });

    $("#moveUp").on("click", function () {
        $('#canvas').cropper('move', 0, -50);
    });

    $("#moveDown").on("click", function () {
        $('#canvas').cropper('move', 0, 50);
    });

    $("#zoomPlus").on("click", function () {
        $('#canvas').cropper('zoom', 0.1);
    });

    $("#zoomMinus").on("click", function () {
        $('#canvas').cropper('zoom', -0.1);
    });

    $("#cropperClose").on("click", cropperClose);
    var body = document.querySelector('body');
    function cropperClose() {
        body.classList.remove("open");
        $(".containerCrop").hide();
        $(".navbar").show();
    }



    $('#img_file').on('change', function () {
        if (this.files && this.files[0]) {
            if (this.files[0].type.match(/^image\//)) {
                var reader = new FileReader();
                
                reader.onload = function (e) {
                    var img = new Image();
                    img.onload = function () {
                        context.canvas.width = img.width;
                        context.canvas.height = img.height;

                        if (img.width <= 300 && img.height<=300) {
                            alert("Ображення менше 300 пікселів")
                            return;
                        }
                        //Показуємо діалог
                        
                        body.classList.toggle("open");
                        $(".containerCrop").show();
                        $(".navbar").hide();

                        context.drawImage(img, 0, 0);
                        var cropper = $canvas.cropper('destroy').cropper({
                            aspectRatio: 1 / 1,
                            viewMode: 1,
                            dragMode: 'move',
                            preview: '.img-preview',
                            //autoCropArea: 0.00000001,
                            //aspectRatio: 2,
                            //,
                            crop: function (e) {
                                var data = e.detail;
                                var h = Math.round(data.height);
                                var w = Math.round(data.width);
                                if (w <= 300) {
                                    this.cropper.setData({ width: 300 });
                                }
                                //else
                            }
                        });
                    }
                    img.src = e.target.result;
                };
                $("#crop").click(function () {
                    var croppedImage = $canvas.cropper('getCroppedCanvas').toDataURL('image/jpg');
                    $('#result').html($('<img>').attr('src', croppedImage));
                    console.log(croppedImage);
                    //Зображення обрізане записуємо у скрите поле на формі
                    $("#imgSelectView").attr("src", croppedImage);
                    $('#ImageBase64').attr("value", croppedImage.split(',')[1]);
                    cropperClose();
                });

                reader.readAsDataURL(this.files[0]);
            }
            else {
                alert("Invalid file type");
            }
        }
        else {
            alert("Please select a file.");
        }

    });
});