import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatDialogRef } from '@angular/material/dialog';
import { environment } from '@environments/environment';
import { ApiResponse, QueryStateEnum } from '@app/app.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {

  indexForm = new FormGroup({});

  response$: ApiResponse;

  constructor(public dialogRef: MatDialogRef<IndexComponent>,
              private httpClient: HttpClient,
              private toastr: ToastrService) {

    this.response$ =  {
      state: QueryStateEnum.Init,
      data: null,
      message: '',
      code: 0
    };

    this.indexForm = new FormGroup({
      url: new FormControl('', [Validators.required ]),
      depth: new FormControl(1, Validators.required)
    });
  }

  ngOnInit(): void {
  }



  onSubmit(): void {

    if (this.indexForm.invalid) {
        return;
    }

    const requestHeaders = new HttpHeaders().set(environment.apikeyHeadername, environment.apiKeyHeaderValue);

    const body = this.buildRequestIndex();

    this.response$.state = QueryStateEnum.Loading;

    this.httpClient
        .post<ApiResponse>(`${environment.apiUrl}/engine/index`, body, {
          headers: requestHeaders
        })
        .subscribe(x => {

          this.response$ = x;
          if (x.code !== 200) {
              this.toastr.error(`Fail indexing page... error ${x.message} 😧`, 'Error', {
                timeOut: 5000
              });
          } else if (x.data.indexedWordsCount === 0) {
            this.toastr.info(`Seems like the given page doesn't exist or is unreadable for us..😒`, 'Error', {
              timeOut: 5000
            });
          } else {
            this.response$.state = QueryStateEnum.Finished;
            this.toastr.success(`Indexed pages ${x.data.indexedPagesCount} and ${x.data.indexedWordsCount} words`, 'Success', {
              timeOut: 5000
            });
            this.dialogRef.close();
          }

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


  buildRequestIndex(): any {
    return  {
      Url: this.indexForm.get('url').value,
      Depth: this.indexForm.get('depth').value
    };
  }

}
