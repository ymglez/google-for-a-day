import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ApiResponse, QueryStateEnum } from '@app/app.model';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-clear',
  templateUrl: './clear.component.html',
  styleUrls: ['./clear.component.scss']
})
export class ClearComponent implements OnInit {

  response$: ApiResponse;

  constructor(
    public dialogRef: MatDialogRef<ClearComponent>,
    private httpClient: HttpClient,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.response$ =  {
      state: QueryStateEnum.Init,
      data: null,
      message: '',
      code: 0
    };
  }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }


  onClick(): void {

    const requestHeaders = new HttpHeaders().set(environment.apikeyHeadername, environment.apiKeyHeaderValue);
    this.response$.state = QueryStateEnum.Loading;

    this.httpClient
        .post<ApiResponse>(`${environment.apiUrl}/engine/clear`, {}, {
          headers: requestHeaders
        })
        .subscribe(x => {

          this.response$ = x;
          if (x.code !== 200) {
              this.toastr.error(`Fail clearing index ${x.message} 😧`, 'Error', {
                timeOut: 5000
              });
          } else {
          this.response$.state = QueryStateEnum.Finished;
          this.toastr.success(`Index cleared`, 'Success', {
            timeOut: 5000
          });
        }
          this.router.navigate(['/search'], { queryParams: { q: '' } });
          this.dialogRef.close();

        },
        err =>  {
          // http error
          this.response$.state = QueryStateEnum.Error;
          this.response$.message = err.message;
          this.toastr.error(`We are very sorry but... ${err.message}. Maybe try again later? 😱`, 'Error', {
            timeOut: 5000
          });
        });
  }

}
