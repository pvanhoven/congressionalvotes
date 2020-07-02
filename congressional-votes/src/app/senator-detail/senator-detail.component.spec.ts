import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SenatorDetailComponent } from './senator-detail.component';

describe('SenatorDetailComponent', () => {
  let component: SenatorDetailComponent;
  let fixture: ComponentFixture<SenatorDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SenatorDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SenatorDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
