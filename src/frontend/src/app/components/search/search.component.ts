import { Component, OnInit, OnDestroy } from '@angular/core';
import { SearchStateEnum, ApiResponse } from '@app/app.model';
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
            of({ state: SearchStateEnum.Loading } as ApiResponse),
            this.doSearch(query.term).pipe(
              map(result => {

                console.log({resultApi:  result});

                if (+result.code !== 200) {
                  throw new Error(result.message);
                }
                result.state = SearchStateEnum.Finished;
                return result;

              }),
              catchError(err => of({ state: SearchStateEnum.Error, message: err } as ApiResponse))
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
