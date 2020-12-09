import { Component, OnInit, OnDestroy } from '@angular/core';
import { QueryStateEnum, ApiResponse } from '@app/app.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Observable, of, concat, Subject } from 'rxjs';
import { map, switchMap, filter, catchError, takeUntil } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { SearchService } from '@app/services/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit, OnDestroy {

  destroy$ = new Subject<void>();

  control = new FormControl();
  query$: Observable<ApiResponse>;

  constructor(private httpClient: HttpClient, private searchService: SearchService) {

  }

  ngOnInit(): void {
    this.query$ = this.searchService.onSearchSubmit()
      .pipe(
        filter(query => !!query.term),
        switchMap(query =>
          concat(
            of({ state: QueryStateEnum.Loading } as ApiResponse),
            this.doSearch(query.term).pipe(
              map(result => {

                if (+result.code !== 200) {
                  throw new Error(result.message);
                }
                result.state = QueryStateEnum.Finished;
                return result;

              }),
              catchError(err => of({ state: QueryStateEnum.Error, message: err.message } as ApiResponse))
            )
          )
        ),
        takeUntil(this.destroy$)
      );
  }

  doSearch(query: string): Observable<ApiResponse> {
    const requestHeaders = new HttpHeaders().set(environment.apikeyHeadername, environment.apiKeyHeaderValue);

    return this.httpClient
      .get<ApiResponse>(`${environment.apiUrl}/engine/search/${query}`, {
        headers: requestHeaders
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
