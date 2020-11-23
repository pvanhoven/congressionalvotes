import { APP_BASE_HREF } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { AppRoutingModule } from '../app-routing.module';
import { HomeComponent } from '../home/home.component';

import { SenatorDetailComponent } from './senator-detail.component';

describe('SenatorDetailComponent', () => {
  let component: SenatorDetailComponent;
  let fixture: ComponentFixture<SenatorDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HomeComponent, SenatorDetailComponent],
      imports: [
        HttpClientModule,
        AppRoutingModule,
        MatProgressSpinnerModule,
        MatInputModule,
        MatSelectModule,
        MatCardModule,
        NgxSkeletonLoaderModule,
      ],
      providers: [{ provide: APP_BASE_HREF, useValue: '/' }],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SenatorDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
