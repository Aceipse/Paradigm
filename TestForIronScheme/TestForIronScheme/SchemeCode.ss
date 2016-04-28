(define (drawCircle x0 y0 r)
   (define f (- 1 r))
   (define ddf_x 1)
   (define ddf_y (* -2 r))
   (define x 0)
   (define y r)
   
   ; Our list of points starts with some defined points  
   (define listToReturn (list `( ,x0 ,(+ y0 r) ) `(,x0 ,(- y0 r) ) `(,(+ x0 r) ,y0) `(,(- x0 r) ,y0)))
   
  ;recursive function to find other coordinates
  (let loop ((x x)(y y)(ddf_y ddf_y)(ddf_x ddf_x)(f f)(x0 x0)(y0 y0)(listToReturn listToReturn))
    
    (display "Called with:\n")
    (display `(x: ,x y: ,y ddf_y: ,ddf_y ddf_x: ,ddf_x f: ,f x0: ,x0 y0: ,y0 listToReturn: ,listToReturn))
    (display "\n")
    
    (if (>= x y)        
      (begin
       (display "Done --------------------------------------------------------\n")
       (display listToReturn)
       listToReturn
        )
      (
       (if (>= f 0)
           
            (let* ((y (- y 1)) (ddf_y (+ ddf_y 2)) (f (+ f ddf_y)))
              (let* ((x (+ x 1)) (ddf_x (+ ddf_x 2)) (f (+ f ddf_x)))
                 (
                  ;(display `(First: x: ,x y: ,y ddf_y: ,ddf_y ddf_x: ,ddf_x f: ,f x0: ,x0 y0: ,y0 listToReturn: ,listToReturn))
 
                  loop x y ddf_y ddf_x f x0 y0 (
                                                 append listToReturn `(
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
              )
            
           
            (let* ((x (+ x 1)) (ddf_x (+ ddf_x 2)))
                       
                  (let* ((f (+ f ddf_x)))
                    (
                     ;(display `(Second: x: ,x y: ,y ddf_y: ,ddf_y ddf_x: ,ddf_x f: ,f x0: ,x0 y0: ,y0 listToReturn: ,listToReturn))
                     loop x y ddf_y ddf_x f x0 y0 (
                                                 append listToReturn `(
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
                  
                  
                   
             )
                      
           
        )
       
     )
   )
  )
)
(define line (lambda (x0 y0 x1 y1 l)
               (define dx (lambda () (abs (- x1 x0))))
               (define dy (lambda () (abs (- y1 y0))))
               (define sx (lambda () (if (> x0 x1) -1 1)))
               (define sy (lambda () (if (> y0 y1) -1 1)))
               
               (define newerr (lambda (err)
                                (- err (dy))
                                )
                 )
               
               (define add-point (lambda (l x y)
                                   (append l (list `(,x ,y)))
                                   )
                 )
               
               (define print-point (lambda (x y)
                                     (display "(")
                                     (display x)
                                     (display ";")
                                     (display y)
                                     (display ")")
                                     (newline)
                                     )
                 )               
               
               (define loop-x (lambda (x y err l)
                                (if (= x x1)
                                    (begin
                                      (display "done")
                                      l
                                      )
                                    (begin
                                      (print-point x y)
                                      
                                      (if (< (newerr err) 0)
                                          (loop-x (+ x (sx)) (+ y (sy)) (+ (newerr err) (dx)) (add-point l x y))
                                          (loop-x (+ x (sx)) y (newerr err) (add-point l x y))
                                          )
                                      )
                                    )
                                )
                 )
               
               (define loop-y (lambda (x y err l)
                                (if (= y y1)
                                    (begin
                                      (display "done")
                                      l
                                      )
                                    (begin
                                      (print-point x y)
                                      
                                      (if (< (newerr err) 0)
                                          (loop-y (+ x (sx)) (+ y (sy)) (newerr err) (add-point l x y))
                                          (loop-y x (+ y (sy)) (+ (newerr err) (dy)) (add-point l x y))
                                          )
                                      )
                                    )
                                )
                 )
               
               (if (> (dx) (dy))
                   (loop-x x0 y0 (/ (dx) 2.0) l)
                   (loop-y x0 y0 (/ (dy) 2.0) l)
                   )
               )
  )