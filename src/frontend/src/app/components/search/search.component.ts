import { Component, OnInit, OnDestroy } from '@angular/core';
import { SearchResults, SearchStateEnum, SearchQuery } from '@app/app.model';
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
  query$: Observable<SearchQuery>;
  

  constructor(private httpClient: HttpClient, private searchService: SearchService) {

  }

  ngOnInit(): void {
    this.query$ = this.searchService.onSearchSubmit()
      .pipe(
        filter(query => !!query.term),
        switchMap(query =>
          concat(
            of({ state: SearchStateEnum.Loading } as SearchQuery),
            this.doSearch(query.term).pipe(
              map(results => ({state: SearchStateEnum.Finished, data: results} as SearchQuery)),
              catchError(err => of({ state: SearchStateEnum.Error, error: err } as SearchQuery))
            )
          )
        ),
        takeUntil(this.destroy$)
      );
  }

  doSearch(query: string): Observable<SearchResults> {
    const requestHeaders = new HttpHeaders().set(environment.apikeyHeadername, environment.apiKeyHeaderValue);

    return this.httpClient
      .get<SearchResults>(`${environment.apiUrl}/engine/search/${query}`, {
        headers: requestHeaders
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
