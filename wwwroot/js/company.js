var dataTable;

$(document).ready(function (){
    loadDataTable();
});

function loadDataTable(){
    dataTable=$('#tblData').DataTable({
        "ajax":{url:'/company/getall'},
        "columns":[
            {data:'name',width:"25%"},
            {data:'city',width:"15%"},
            {data:'state',width:"10%"},
            {data:'postalCode',width:"10%"},
            {data:'phoneNumber',width:"15%"},
            {data:'companyId',width:"25%",
                render: function (data){
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/Company/Upsert/${data}" class="btn btn-primary mx-4"><i
                        class="bi bi-pencil-square"></i> Edit </a>
                    <a onClick=Delete('/Company/Delete/${data}') class="btn btn-danger mx-4"><i
                        class="bi bi-trash-fill"></i> Delete </a>`
                }
            }
        ]

    });
}

function Delete(url){
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url:url,
                type:"DELETE",
                success:function(data){
                    dataTable.ajax.reload();
                    toastr(data.message);
                }
            })
        }
    });
}