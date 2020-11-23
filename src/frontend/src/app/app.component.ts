import { Component } from '@angular/core';

import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { IndexComponent } from './components/index/index.component';
import { SearchService } from './services/search.service';
import { ClearComponent } from './components/clear/clear.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  destroy$: Observable<void>;

  control = new FormControl();

  query$: string;

  constructor(private route: ActivatedRoute,
              private searchService: SearchService,
              private dialog: MatDialog,
              private router: Router) {

  }

  // tslint:disable-next-line: use-lifecycle-interface
  ngOnInit(): void {

    this.route.queryParams
      .pipe(
        map(params => params.q),
        filter(query => query)
      )
      .subscribe(query => {
        this.control.setValue(query);
        this.searchService.submitSearch(query);
      });

  }

  onKeyupEnter(value: string): void {
    this.query$ = value;

    // this.searchService.submitSearch(value);

    // Instead of firing the Search directly, let's update the Route instead:
    this.router.navigate(['/search'], { queryParams: { q: value } });
  }

  openIndexDialog() {
    this.dialog.open(IndexComponent);
  }

  openClearDialog() {
    this.dialog.open(ClearComponent);
  }

}
