(define Min (cons 0 0))
(define Max (cons 0 0))

(define (IsHeigher value)
  (if (>= (car value) (car Min))   
   (if(>= (cadr value) (cdr Min))
    #t #f)
   #f)
)

(define (IsLower value)
  (if(<= (car value) (car Max))
    (if(<= (cadr value) (cdr Max))
    #t #f) 
   #f)
)

(define (Limiter l)
 (filter IsHeigher (filter IsLower l))
)


(define (drawCircle x0 y0 r)
   (define f (- 1 r))
   (define ddf_x 1)
   (define ddf_y (* -2 r))
   (define x 0)
   (define y r)
   (define addRecursivePoints (lambda (l x y x0 y0)
                                   (append l `(
                                                                       (,(+ x0 x) ,(+ y0 y))
                                                                       (,(- x0 x) ,(+ y0 y))
                                                                       (,(+ x0 x) ,(- y0 y))
                                                                       (,(- x0 x) ,(- y0 y))
                                                                       (,(+ x0 y) ,(+ y0 x))
                                                                       (,(- x0 y) ,(+ y0 x))
                                                                       (,(+ x0 y) ,(- y0 x))
                                                                       (,(- x0 y) ,(- y0 x))
                                                                       )
                                     )
                                   )
     )
   
  
   ; Our list of points starts with some defined points

  
  (define initialList (list `( ,x0 ,(+ y0 r) ) `(,x0 ,(- y0 r) ) `(,(+ x0 r) ,y0) `(,(- x0 r) ,y0)))
  
  (define displayCallValues (lambda  (x y ddf_y ddf_x f x0 y0 listToReturn)
    (begin
      (display "Called with:\n")
    (display `(x: ,x y: ,y ddf_y: ,ddf_y ddf_x: ,ddf_x f: ,f x0: ,x0 y0: ,y0 listToReturn: ,listToReturn))
    (display "\n")
      )                              
                             )
    )
  
  
  
  (define firstLet (lambda (x y ddf_y ddf_x f x0 y0 listToReturn)
                    (let* ((y (- y 1)) (ddf_y (+ ddf_y 2)) (f (+ f ddf_y)) (x (+ x 1)) (ddf_x (+ ddf_x 2)) (f (+ f ddf_x)))
                 (loop x y ddf_y ddf_x f x0 y0 (addRecursivePoints listToReturn x y x0 y0))               
             ) 
                     )
    )
  
    (define secondLet (lambda (x y ddf_y ddf_x f x0 y0 listToReturn)
                     (let* ((x (+ x 1)) (ddf_x (+ ddf_x 2)) (f (+ f ddf_x)))                    
                 (loop x y ddf_y ddf_x f x0 y0 (addRecursivePoints listToReturn x y x0 y0))    
             )
                     )
      )
  
  (define displayDoneAndReturn(lambda (listToReturn)

       (begin
         (display "---------------------- Done -------------------------\n")
         (display listToReturn)
         listToReturn
         )
         
        
                                )
    )
  
  (define loop (lambda (x y ddf_y ddf_x f x0 y0 listToReturn)

                     ;(displayCallValues x y ddf_y ddf_x f x0 y0 listToReturn)
    
    (if (>= x y)        
      ;(displayDoneAndReturn listToReturn)
      listToReturn
       ;(<--- FJERNET!
       (if (>= f 0)           
            (firstLet x y ddf_y ddf_x f x0 y0 listToReturn)
            (secondLet x y ddf_y ddf_x f x0 y0 listToReturn)
           )
       ;)<----FJERNET!
        )
                     )
    )
  
  
  
  ;(displayDoneAndReturn '(1 2))
  (loop x y ddf_y ddf_x f x0 y0 initialList)
    
)

(define line
  (lambda (x0 y0 x1 y1)
    (define dx (- x1 x0))
    (define dy (- y1 y0))
    
    (define clockwise
      (lambda (x y)
        (list y (- x))))
    
    (define counter-clockwise
      (lambda (x y)
        (list (- y) x)))
    
    (define no-rotate
      (lambda (x y)
        (list x y)))
    
    (define rad2deg
      (lambda (rad)
        (let ((pi 3.1415926535897932))
          (/ (* 180 rad) pi))))
    
    (define atan-deg
      (lambda (x y)
        (if (and (= x 0) (= y 0))
            0
            (rad2deg (atan x y)))))
    
    (define octo
      (lambda (x y)
        (let ((deg (atan-deg x y)))
          (if (and (< deg 45) (> deg -45))
              ;We're in Q1 or Q2 rotate clockwise 90 deg
              (line-aux (car (clockwise x0 y0)) (cadr (clockwise x0 y0))
                        (car (clockwise x1 y1)) (cadr (clockwise x1 y1))
                        '() counter-clockwise)
              (if (and (>= deg 45) (<= deg 135))
                  ;We're in Q0 or Q7 do nothing
                  (line-aux x0 y0 x1 y1 '() no-rotate)
                  (if (or (> deg 135) (<= deg -135))
                      ;We're in Q6 or Q5 rotate counter-clockwise
                      (line-aux (car (counter-clockwise x0 y0)) (cadr (counter-clockwise x0 y0))
                                (car (counter-clockwise x1 y1)) (cadr (counter-clockwise x1 y1))
                                '() clockwise)
                      ;We're in Q4 or Q3 do nothing
                      (line-aux x0 y0 x1 y1 '() no-rotate)
                      ))))))
    
    (octo dx dy)
    ))

(define line-aux
  (lambda (x0 y0 x1 y1 l transform)
    (define dx (lambda () (abs (- x1 x0))))
    (define dy (lambda () (abs (- y1 y0))))
    (define sx (lambda () (if (> x0 x1) -1 1)))
    (define sy (lambda () (if (> y0 y1) -1 1)))
    
    (define newerr
      (lambda (err)
        (- err (dy))))
    
    (define add-point
      (lambda (l x y)
        (append l (list (transform x y)))))
    
    (define loop-x
      (lambda (x y err l)
        (if (= x x1)
            l 
            (if (< (newerr err) 0)
                (loop-x (+ x (sx)) (+ y (sy)) (+ (newerr err) (dx)) (add-point l x y))
                (loop-x (+ x (sx)) y (newerr err) (add-point l x y))))))
    
    (define loop-y
      (lambda (x y err l)
        (if (= y y1)
            l
            (if (< (newerr err) 0)
                (loop-y (+ x (sx)) (+ y (sy)) (newerr err) (add-point l x y))
                (loop-y x (+ y (sy)) (+ (newerr err) (dy)) (add-point l x y))))))
    
    (if (> (dx) (dy))
        (loop-x x0 y0 (/ (dx) 2) l)
        (loop-y x0 y0 (/ (dy) 2) l)))
  )

(define rectangle
  (lambda (x0 y0 x1 y1)
    (append (append (append (append '() (line x0 y0 x0 y1)) (line x0 y0 x1 y0)) (line x0 y1 x1 y1)) (line x1 y0 x1 y1))))

(
 define (fill-rectangle color x0 y0 x1 y1)
  
    (let loopx ((x0x x0)
               (y0x y0)
               (x1x x1)
               (y1x y1)
               (listToReturn '())                
                )
      (if (> x0x x1x)
          ;Return the list with the color
          ;not adding the color here anymore (cons color listToReturn)
		  listToReturn
 (let loopy ((x0y x0x)
               (y0y y0x)
               (x1y x1x)
               (y1y y1x)
               (listToReturn listToReturn)                
                )
   (if (> y0y y1y)
       (loopx (+ x0x 1) y0x x1x y1x listToReturn)
       
        ;Add point and call y loop again
        
        (loopy x0y (+ y0y 1) x1y y1y (append listToReturn (list `(,x0y ,y0y))))

      
      )
          )
      
      )
  
  )
)

(define (fill-circle color x y r)
  
  ;make a retangle around the circle
  
  (define x0 (- x r))
  (define y0 (- y r))
  (define x1 (+ x r))
  (define y1 (+ y r))
  
  (define myRec (fill-rectangle color x0 y0 x1 y1))
  
 
   (define (withinCircle coordinate)
     (display coordinate)

    (if(< (sqrt (+(expt (- (car coordinate) x) 2)(expt (- (cadr coordinate) y) 2))) r)
      #t
	  #f
      )
    
    )
  
  (display myRec)
  
  (filter withinCircle (cdr myRec))
  
  
 )

(
 define(EvalFunc x)
 (if(and (equal? Min Max)(not(equal? (car x) 'BOUNDING-BOX)))
    "ERROR Bounding box have not been made"
  (cond ((equal? (car x) 'LINE)
  `(("FIGURE") ("Black")
         ,(Limiter(line
           (caadr x)
           (cadadr x)
           (caaddr x)
           (car(cdaddr x)))))
         
         )

        ((equal? (car x) 'RECTANGLE)
		(begin
		"RECTANGLE CALLED"
		(list (list "FIGURE") (list "BLACK") (Limiter(rectangle (caadr x) (cadadr x) (caaddr x) (car (cdaddr x)))))))

    ((equal? (car x) 'CIRCLE)
     
     `(("FIGURE") ("Black")
     ,(Limiter(drawCircle
       (caadr x)
       (cadadr x)
       (caddr x)))))
    
    
    ((equal? (car x) 'TEXT-AT) `(("TEXT")(,(caddr x))(,(caadr x),(cadadr x))))
    ((equal? (car x) 'BOUNDING-BOX) 
     
	  (set! Min(cons  (caadr x) (cadadr x)))
	  (set! Max(cons (caaddr x) (car(cdaddr x))))
     ;(BOUNDING-BOX 
     ;  "box"
     ;  (caadr x)
     ;  (cadadr x)
     ;  (caaddr x)
     ;  (car(cdaddr x)))
     
     )
    ((equal? (car x) 'DRAW) "Draw was called")
    ((equal? (car x) 'FILL)
    ;Check which fill function it was, only circle and rectangle allowed
     
     (cond ((equal? (caaddr x) 'RECTANGLE)
            `(("FIGURE")(,(cadr x))
            ,(fill-rectangle 
              (cadr x)  
              (caadr(caddr x))
              (cadadr(caddr x))
              (caaddr(caddr x))
              (cadr(caddr(caddr x)))))
            )
           ((equal? (caaddr x) 'CIRCLE)
            ;This function could make a call to fill circle 
            ;and remove all points not in distance of the midpoint
             `(("FIGURE")(,(cadr x))
            ,(fill-circle
              (cadr x)  
              (caadr(caddr x))
              (cadadr(caddr x))
              (caddr(caddr x)))))
           (else '(("ERROR")("Cannot fill that figure")))))
    (else '(("ERROR")("Invalid function call"))))
))
          

(define (DRAW color toDraw)
  (cons color (map (lambda (x) (EvalFunc x)) toDraw))
)