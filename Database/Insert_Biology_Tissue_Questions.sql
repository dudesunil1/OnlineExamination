-- =============================================================================
-- Insert 10 Biology Tissue Questions
-- =============================================================================
-- This script adds 10 sample questions about Biology - Tissue topic
-- Run this in SQL Server Management Studio after ensuring subjects/topics exist
-- =============================================================================

USE OnlineExamination;
GO

-- First, ensure we have Biology subject, Class, Topic, and Publication
-- You may need to adjust these IDs based on your database

DECLARE @SubjectId INT = (SELECT TOP 1 Sub_Id FROM SubjectMaster WHERE Sub_Name LIKE '%Biology%');
DECLARE @ClassId INT = (SELECT TOP 1 ID FROM ClassMaster WHERE Name LIKE '%11%' OR Name LIKE '%12%');
DECLARE @TopicId INT = (SELECT TOP 1 Top_Id FROM TopicMaster WHERE Top_Name LIKE '%Tissue%');
DECLARE @PubId INT = (SELECT TOP 1 Pub_Id FROM PublicationMaster WHERE Pub_IsActive = 1);

-- If not found, create them
IF @SubjectId IS NULL
BEGIN
    INSERT INTO SubjectMaster (Sub_Name, Sub_Description, Sub_IsActive) 
    VALUES ('Biology', 'Biological Sciences', 1);
    SET @SubjectId = SCOPE_IDENTITY();
END

IF @ClassId IS NULL
BEGIN
    INSERT INTO ClassMaster (Name, Description, IsActive) 
    VALUES ('Class 11', 'Class 11', 1);
    SET @ClassId = SCOPE_IDENTITY();
END

IF @PubId IS NULL
BEGIN
    INSERT INTO PublicationMaster (Pub_Name, Pub_Description, Pub_IsActive) 
    VALUES ('NCERT', 'National Council of Educational Research and Training', 1);
    SET @PubId = SCOPE_IDENTITY();
END

IF @TopicId IS NULL
BEGIN
    INSERT INTO TopicMaster (Top_Name, Top_SubId, Top_ClassID, Top_Description, Top_IsActive) 
    VALUES ('Tissue', @SubjectId, @ClassId, 'Study of tissues in plants and animals', 1);
    SET @TopicId = SCOPE_IDENTITY();
END

PRINT 'Subject ID: ' + CAST(@SubjectId AS VARCHAR(10));
PRINT 'Class ID: ' + CAST(@ClassId AS VARCHAR(10));
PRINT 'Topic ID: ' + CAST(@TopicId AS VARCHAR(10));
PRINT 'Publication ID: ' + CAST(@PubId AS VARCHAR(10));
PRINT '';

-- =============================================================================
-- Insert 10 Biology Tissue Questions
-- =============================================================================

-- Question 1
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>Which tissue is responsible for the growth of plants in length?</p>',
    '<p>Apical meristem</p>',
    '<p>Lateral meristem</p>',
    '<p>Intercalary meristem</p>',
    '<p>Permanent tissue</p>',
    '<p><strong>Explanation:</strong> Apical meristem is present at the growing tips of stems and roots and is responsible for increase in length of the plant.</p>',
    1
);
PRINT 'Question 1 inserted: Apical meristem';

-- Question 2
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>What type of epithelial tissue is found in the inner lining of blood vessels?</p>',
    '<p>Squamous epithelium</p>',
    '<p>Cuboidal epithelium</p>',
    '<p>Columnar epithelium</p>',
    '<p>Ciliated epithelium</p>',
    '<p><strong>Explanation:</strong> Squamous epithelium consists of thin, flat cells and forms the lining of blood vessels, air sacs of lungs, and other areas where diffusion needs to occur.</p>',
    1
);
PRINT 'Question 2 inserted: Squamous epithelium';

-- Question 3
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>Which connective tissue connects bones to bones?</p>',
    '<p>Ligament</p>',
    '<p>Tendon</p>',
    '<p>Cartilage</p>',
    '<p>Areolar tissue</p>',
    '<p><strong>Explanation:</strong> Ligaments are dense fibrous connective tissue that connect bone to bone at joints. Tendons connect muscle to bone.</p>',
    1
);
PRINT 'Question 3 inserted: Ligament';

-- Question 4
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>Xylem and phloem are types of which plant tissue?</p>',
    '<p>Complex permanent tissue</p>',
    '<p>Simple permanent tissue</p>',
    '<p>Meristematic tissue</p>',
    '<p>Epidermal tissue</p>',
    '<p><strong>Explanation:</strong> Xylem and phloem are complex permanent tissues made up of more than one type of cells working together as a unit. They are responsible for conduction of water and food respectively.</p>',
    1
);
PRINT 'Question 4 inserted: Complex permanent tissue';

-- Question 5
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>Which muscle tissue is found in the heart?</p>',
    '<p>Cardiac muscle</p>',
    '<p>Smooth muscle</p>',
    '<p>Striated muscle</p>',
    '<p>Voluntary muscle</p>',
    '<p><strong>Explanation:</strong> Cardiac muscle is found only in the heart. It is involuntary, striated, and branched. It contracts rhythmically throughout life.</p>',
    1
);
PRINT 'Question 5 inserted: Cardiac muscle';

-- Question 6
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>What is the function of adipose tissue?</p>',
    '<p>Storage of fat</p>',
    '<p>Support and binding</p>',
    '<p>Transportation</p>',
    '<p>Protection from pathogens</p>',
    '<p><strong>Explanation:</strong> Adipose tissue is a specialized connective tissue that stores fat. It acts as insulation, cushioning, and energy reserve.</p>',
    1
);
PRINT 'Question 6 inserted: Adipose tissue';

-- Question 7
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>Which tissue forms the brain and spinal cord?</p>',
    '<p>Nervous tissue</p>',
    '<p>Epithelial tissue</p>',
    '<p>Muscular tissue</p>',
    '<p>Connective tissue</p>',
    '<p><strong>Explanation:</strong> Nervous tissue is specialized for receiving stimuli and conducting impulses. It forms the brain, spinal cord, and nerves.</p>',
    1
);
PRINT 'Question 7 inserted: Nervous tissue';

-- Question 8
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>Which tissue is responsible for photosynthesis in plants?</p>',
    '<p>Chlorenchyma</p>',
    '<p>Parenchyma</p>',
    '<p>Collenchyma</p>',
    '<p>Sclerenchyma</p>',
    '<p><strong>Explanation:</strong> Chlorenchyma is a type of parenchyma tissue that contains chloroplasts and performs photosynthesis. It is found in leaves and green stems.</p>',
    1
);
PRINT 'Question 8 inserted: Chlorenchyma';

-- Question 9
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>What is the matrix of blood called?</p>',
    '<p>Plasma</p>',
    '<p>Serum</p>',
    '<p>Lymph</p>',
    '<p>Interstitial fluid</p>',
    '<p><strong>Explanation:</strong> Blood is a fluid connective tissue. Its matrix is called plasma, which is a pale yellow liquid composed mainly of water with dissolved substances.</p>',
    1
);
PRINT 'Question 9 inserted: Plasma';

-- Question 10
INSERT INTO QuestionMaster (
    Ques_SubId, Ques_ClassId, Ques_TopId, Ques_PubId,
    Ques_Mark, Ques_JEEMark, Ques_Negative,
    Ques_Question, Ques_Answer, Ques_OptionB, Ques_OptionC, Ques_OptionD,
    Ques_SolutionDetails, Ques_IsActive
)
VALUES (
    @SubjectId, @ClassId, @TopicId, @PubId,
    1, 4, 0.25,
    '<p>Which tissue provides flexibility to plants?</p>',
    '<p>Collenchyma</p>',
    '<p>Sclerenchyma</p>',
    '<p>Parenchyma</p>',
    '<p>Meristem</p>',
    '<p><strong>Explanation:</strong> Collenchyma tissue provides mechanical support and flexibility to young plant parts. It is found in leaf stalks below the epidermis.</p>',
    1
);
PRINT 'Question 10 inserted: Collenchyma';

PRINT '';
PRINT '=============================================================================';
PRINT 'Successfully inserted 10 Biology Tissue questions!';
PRINT '=============================================================================';
PRINT '';
PRINT 'Questions Summary:';
PRINT '1. Apical meristem - Plant growth';
PRINT '2. Squamous epithelium - Blood vessel lining';
PRINT '3. Ligament - Connects bones';
PRINT '4. Complex permanent tissue - Xylem and phloem';
PRINT '5. Cardiac muscle - Heart tissue';
PRINT '6. Adipose tissue - Fat storage';
PRINT '7. Nervous tissue - Brain and spinal cord';
PRINT '8. Chlorenchyma - Photosynthesis';
PRINT '9. Plasma - Blood matrix';
PRINT '10. Collenchyma - Plant flexibility';
PRINT '';
PRINT 'You can now use these questions in your tests!';
PRINT '';

GO
