function initTable() {
    $('#cars-data-table').bootstrapTable(
        {
            columns: [
                {
                    title: 'Car name',
                    field: 'name',
                },
                {
                    title: 'Description',
                    field: 'description',
                },
                {
                    title: 'Year',
                    field: 'year',
                },
                {
                    title: 'Producer',
                    field: 'producer.name',
                },
                {
                    title: 'Deleted',
                    field: 'isDeleted',
                },
                {
                    field: 'operate',
                    title: 'Item Operate',
                    align: 'center',
                    clickToSelect: false,
                    events: window.operateEvents,
                    formatter: editDeleteFormatter
                }
            ],
            onLoadSuccess: function () {
                $('a.delete').click(function () {
                    let id = $(this).attr('data-id');
                    let name = $(this).attr('data-name');
                    let deleteModal = $('#delete-modal');

                    deleteModal.find('#delete-car-button').attr('data-id', id);
                    deleteModal.find('#delete-modal-title').text(name);

                    deleteModal.modal('show');
                })
            }
        });
}

function editDeleteFormatter(value, row, index) {
    return [
        '<a href="cars/edit/' + row.id + '" title="Like">Edit</a>  ',
        '<a class="delete" href="javascript:void(0)" title="Like" data-id="' + row.id + '" data-name="' + row.name + '">Delete</a>  '
    ].join('')
}


$(document).ready(function () {
    initTable()

    $('#delete-car-button').click(function () {
        let id = $(this).attr('data-id');
        $.ajax({
            url: '/cars/delete/' + id,
            type: 'DELETE',
            //data: JSON.stringify(id),
            success: function (result) {
                $('#cars-data-table').bootstrapTable('refresh');
                $("#delete-modal").modal('hide');
                // Do something with the result
            }
        });
    });

});