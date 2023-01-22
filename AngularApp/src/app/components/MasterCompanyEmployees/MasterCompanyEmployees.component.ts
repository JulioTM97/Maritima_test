import { Component } from "@angular/core";

@Component({
    selector:'MasterCompanyEmployees',
    templateUrl: './MasterCompanyEmployees.component.html'
})
export class MasterCompanyEmployees {
    public urlToRequest = "https://localhost:7025/api/Employee/Filtered";

    ngOnInit(){
        const request = new XMLHttpRequest();
        request.addEventListener("load", onLoadRequest);
        request.open("GET", this.urlToRequest);
        request.send();

        function onLoadRequest() {
            let _response = JSON.parse(request.responseText);
            let tBody = document.getElementById("tableBody");
            let tableContent = ``;
            for (const i of _response) {
                tableContent += `<tr>
                <td>${i.name}</td>
                <td>${i.lastName}</td>
                <td>${i.document}</td>
                <td>${i.salary}</td>
                <td>${i.gender}</td>
                <td>${i.startDate}</td>
                </tr>`;
            }
            if(tBody)tBody.innerHTML = tableContent;
        }
    }
}