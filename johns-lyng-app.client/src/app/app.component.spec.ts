import { TestBed, ComponentFixture } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { of, throwError } from 'rxjs';
import { TodoItem } from 'src/app/interfaces/app.interface';
import { TodoDataService } from 'src/app/services/todo/todo.service';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let todoServiceMock: jasmine.SpyObj<TodoDataService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('TodoDataService', ['getItems', 'addItem', 'markAsComplete', 'generateAiResponse']);

    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      providers: [
        { provide: TodoDataService, useValue: spy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    todoServiceMock = TestBed.inject(TodoDataService) as jasmine.SpyObj<TodoDataService>;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should call getItems on initialization', () => {
      spyOn(component, 'getItems');
      component.ngOnInit();
      expect(component.getItems).toHaveBeenCalled();
    });
  });

  describe('getItems', () => {
    it('should load items and separate completed and incomplete items', () => {
      const mockItems: TodoItem[] = [
        { id: '1', description: 'Task 1', isCompleted: false },
        { id: '2', description: 'Task 2', isCompleted: true }
      ];
      todoServiceMock.getItems.and.returnValue(of(mockItems));

      component.getItems();

      expect(component.items.length).toBe(1);
      expect(component.completedItems.length).toBe(1);
      expect(component.errors).toBe('');
    });

    it('should handle errors', () => {
      const mockError = 'Error occurred';
      todoServiceMock.getItems.and.returnValue(throwError(mockError));

      component.getItems();

      expect(component.errors).toBe(mockError);
    });
  });

  describe('addItem', () => {
    it('should call getItems and handleClear on success', () => {
      spyOn(component, 'getItems');
      spyOn(component, 'handleClear');
      todoServiceMock.addItem.and.returnValue(of({}));

      component.addItem();

      expect(component.getItems).toHaveBeenCalled();
      expect(component.handleClear).toHaveBeenCalled();
    });

    it('should handle errors', () => {
      const mockError = 'Error occurred';
      todoServiceMock.addItem.and.returnValue(throwError(mockError));

      component.addItem();

      expect(component.errors).toBe(mockError);
    });
  });

  describe('markAsComplete', () => {
    it('should call getItems on success', () => {
      spyOn(component, 'getItems');
      todoServiceMock.markAsComplete.and.returnValue(of({}));

      component.markAsComplete({ id: '1', description: 'Task 1', isCompleted: false });

      expect(component.getItems).toHaveBeenCalled();
    });

    it('should handle errors', () => {
      const mockError = 'Error occurred';
      todoServiceMock.markAsComplete.and.returnValue(throwError(mockError));

      component.markAsComplete({ id: '1', description: 'Task 1', isCompleted: false });

      expect(component.errors).toBe(mockError);
    });
  });
    

  describe('handleClear', () => {
    it('should clear the description', () => {
      component.description = 'Task description';
      component.handleClear();
      expect(component.description).toBe('');
    });
  });
   

  describe('clearError', () => {
    it('should clear errors', () => {
      component.errors = 'Some error';
      component.clearError();
      expect(component.errors).toBe('');
    });
  });
});
