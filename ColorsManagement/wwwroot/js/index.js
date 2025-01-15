$(document).ready(function () {
  const apiUrl = "http://localhost:5007/api/colors";

  const colorsMap = {
    אדום: "#DC0808",
    כחול: "#2FA2F4",
    ירוק: "#4CAF50",
    צהוב: "#FFFF00",
    לבן: "#FFFFFF",
    ורוד: "#FFC0CB",
    כתום: "#FFA500",
  };

  Object.keys(colorsMap).forEach(function (key) {
    const option = `<option value="${key}">${key}</option>`;
    $("#colorName").append(option);
  });

  $("#colorName").change(function () {
    const selectedColor = $(this).val();
    if (colorsMap[selectedColor]) {
      $("#colorDisplay")
        .css("background-color", colorsMap[selectedColor])
        .css("display", "block")
        .text(`${colorsMap[selectedColor]}`);
    } else {
      $("#colorDisplay")
        .css("background-color", "transparent")
        .css("display", "none")
        .text("");
    }
  });

  function fetchColors() {
    $.ajax({
      url: apiUrl,
      type: "GET",
      success: function (data) {
        $("#message").empty();
        $("#colorTable tbody").empty();
        $("#colorTable tfoot").empty();

        data.sort(function (a, b) {
          return a.displayOrder - b.displayOrder;
        });

        data.forEach(function (color) {
          const row = `<tr data-id="${
            color.id
          }" class="draggable" draggable="true">
                        <td>${color.inStock ? "כן" : "לא"}</td>
                        <td>${color.colorName}</td>
                        <td>${color.price}</td>
                        <td class="color-display" style="background-color:${
                          colorsMap[color.colorName]
                        };">${colorsMap[color.colorName]}</td>
                        <td>${color.displayOrder}</td>
                        <td class="buttons-container">
                            <button class="edit-btn">
                                <i class="fa-solid fa-file-alt"></i> עריכה
                            </button>
                            <button class="delete-btn">
                                <i class="fa-solid fa-times-circle"></i> מחיקה
                            </button>
                        </td>
                    </tr>`;
          $("#colorTable tbody").append(row);
        });

        const totalResultsRow = `<tr><td colspan="6">סך הכל תוצאות: ${data.length}</td></tr>`;
        $("#colorTable tfoot").append(totalResultsRow);
        $("#colorTable").show();
      },
      error: function () {
        $("#colorTable").hide();
        $("#message").text("אין תוצאות להציג");
      },
    });
  }

  function resetForm() {
    $("#colorForm")[0].reset();
    $("#submit-btn").show();
    $("#update-btn").hide();
    $("#reset-btn").hide();
    $("#colorDisplay")
      .css("background-color", "transparent")
      .css("display", "none")
      .text("");
    $("#formError").text("").hide();
  }

  $("#colorForm #submit-btn").click(function (e) {
    e.preventDefault();
    const colorData = {
      colorName: $("#colorName").val(),
      price: $("#price").val(),
      displayOrder: $("#displayOrder").val(),
      inStock: $("#inStock").is(":checked"),
    };

    const validationResult = validateColorData(colorData);

    if (!validationResult.isValid) {
      $("#formError").text(validationResult.message).show();
    } else {
      $.ajax({
        url: `${apiUrl}/create`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(colorData),
        success: function () {
          fetchColors();
          resetForm();
        },
        error: function (xhr, status, error) {
          $("#formError")
            .text(xhr.responseText || error)
            .show();
        },
      });
    }
  });

  $("#colorForm #update-btn").click(function (e) {
    e.preventDefault();
    const colorName = $("#colorName").val();
    const colorId = $("#colorId").val();

    const colorData = {
      colorName: colorName,
      price: $("#price").val(),
      displayOrder: $("#displayOrder").val(),
      inStock: $("#inStock").is(":checked"),
    };
    const validationResult = validateColorData(colorData);
    if (!validationResult.isValid) {
      $("#formError").text(validationResult.message).show();
    } else {
      $.ajax({
        url: `${apiUrl}/update/${colorId}`,
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify(colorData),
        success: function (response) {
          fetchColors();
          resetForm();
        },
        error: function (xhr, status, error) {
          $("#formError")
            .text(xhr.responseJSON || error)
            .show();
        },
      });
    }
  });

  $("#colorTable").on("click", ".edit-btn", function () {
    const row = $(this).closest("tr");
    const colorId = row.data("id");
    $("#formError").text("").hide();
    $("#colorId").val(colorId);
    $("#colorName").val(row.find("td:nth-child(2)").text().trim()).change();
    $("#price").val(row.find("td:nth-child(3)").text().trim());
    $("#displayOrder").val(row.find("td:nth-child(5)").text().trim());
    $("#inStock").prop(
      "checked",
      row.find("td:nth-child(1)").text().trim() === "כן"
    );

    $("#update-btn").show();
    $("#submit-btn").hide();
    $("#reset-btn").show();
  });

  $("#colorTable").on("click", ".delete-btn", function () {
    const row = $(this).closest("tr");
    const colorId = row.data("id");
    $.ajax({
      url: `${apiUrl}/delete/${colorId}`,
      type: "DELETE",
      success: function (response) {
        row.remove();
        fetchColors();
      },
      error: function (xhr, status, error) {
        $("#formError")
          .text(xhr.responseText || error)
          .show();
      },
    });
  });

  $("#reset-btn").click(function () {
    resetForm();
  });

  $("#colorTable tbody").sortable({
    update: function (event, ui) {
      const rows = $(this).children("tr").toArray();
      const updatedOrder = rows.map((row, index) => {
        $(row)
          .find("td:nth-child(5)")
          .text(index + 1);
        return {
          id: $(row).data("id"),
          displayOrder: index + 1,
        };
      });

      $.ajax({
        url: `${apiUrl}/update-order`,
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify({ items: updatedOrder }),
        success: function () {
          fetchColors();
        },
        error: function (xhr, status, error) {
          $("#formError")
            .text(xhr.responseText || error)
            .show();
        },
      });
    },
  });

  function validateColorData(colorData) {
    if (!colorData.colorName || colorData.colorName === "בחר צבע") {
      return { isValid: false, message: "אנא בחר צבע" };
    }
    if (!colorData.price || colorData.price.trim() === "") {
      return { isValid: false, message: "אנא הכנס מחיר" };
    }
    if (!colorData.displayOrder || colorData.displayOrder.trim() === "") {
      return { isValid: false, message: "אנא הכנס סדר הצגה" };
    }
    if (colorData.inStock === undefined) {
      return { isValid: false, message: "אנא סמן אם במלאי" };
    }

    return { isValid: true };
  }

  fetchColors();
});
