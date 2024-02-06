var dataTable;

$(document).ready(function (){
    loadDataTable();
});

function loadDataTable(){
    dataTable=$('#tblData').DataTable({
        "ajax":{url:'/product/getall'},
        "columns":[
            {data: 'title',width:"25%"},
            {data:'isbn',width:"15%"},
            {data:'author',width:"15%"},
            {data:'listPrice',width:"10%"},
            {data:'category.name',width:"10%"},
            {data:'productId',width:"25%",
                render: function (data){
                return `<div class="w-75 btn-group" role="group">
                    <a href="/Product/Upsert/${data}" class="btn btn-primary mx-4"><i
                        class="bi bi-pencil-square"></i> Edit </a>
                    <a onClick=Delete('/Product/Delete/${data}') class="btn btn-danger mx-4"><i
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