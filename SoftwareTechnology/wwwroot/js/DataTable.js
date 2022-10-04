var dataTable;

$(document).ready(function () {
    loadDataTable();
    
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        
        "language": {
            "semptyTable": "Δεν βρέθηκαν δεδομένα",
            "sProcessing": "Επεξεργασία...",
            "sLengthMenu": "Εμφάνισε _MENU_ εγγραφές",
            "sZeroRecords": "Δεν βρέθηκαν δεδομένα",

            "sInfo": "Εμφάνιση _START_ έως _END_ από τις _TOTAL_ εγγραφές",
            "sInfoEmpty": "Εμφάνιση 0 έως 0 από τις 0 εγγραφές",
            "sInfoFiltered": "",
            "sInfoPostFix": "",
            "sSearch": "Αναζήτηση:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Φόρτωση...",
            "oPaginate": {
                "sFirst": "Πρώτο",
                "sLast": "Τελευταίο",
                "sNext": "Επόμενο",
                "sPrevious": "Προηγούμενο"
            }
        },
        "width": "100%"
    });
   
}

